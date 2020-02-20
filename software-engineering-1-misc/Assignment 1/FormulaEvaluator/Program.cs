using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaEvaluator
{
    /// <summary>
    /// This class houses a single method, Evaluate, which evalutes expressions
    /// </summary>
    public static class Evaluator
    {
        // delegate for looking up variables
        public delegate int Lookup(String v);

        /// <summary>
        /// Evaluate an infix expression.
        /// 
        /// Variables are allowed and one must provide a delegate for dereferencing variables.
        /// Legal tokens include: the four operator symbols (+ - * /), left parentheses, right parentheses,
        /// non-negative integers, whitespace, and variables consisting of one or more letters followed by
        /// one or more digits.
        /// If any illegal tokens are passed in the expression, then an ArgumentException is thrown
        /// </summary>
        /// 
        /// <param name="exp">
        /// The infix expression to evaluate.
        /// See summary for a description of the format.    
        /// </param>
        /// 
        /// <param name="variableEvaluator">
        /// The delegate to dereference any variable names.
        /// 
        /// Takes a string as its single argument and returns an int. The string is the variable to lookup,
        /// and it returns the value of the variable.
        /// If the variable cannot be dereferenced, then an ArgumentException is thrown.
        /// </param>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            // split the expression into tokens (taken from assignment)
            // produces some empty strings in the array, those are ignored
            String[] tokens = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            // stacks for handling operators and operands during processing
            Stack<String> operatorStack = new Stack<string>();
            Stack<String> operandStack = new Stack<string>();

            // process each token
            foreach (var token in tokens)
            {
                // remove trailing, leading whitespace from tokens for processing
                processToken(token.Trim(), operatorStack, operandStack, variableEvaluator);
            }

            // process results according to assignment
            if (operatorStack.Count() == 0)
            {
                // return evaluated expression or throw exception if there is none
                if (!(operandStack.Count() == 1))
                {
                    throw new ArgumentException("Error after processing, operator stack empty" +
                        "but operand stack doesn't contain exactly one value");
                }
                
                return int.Parse(operandStack.Pop());
            }
            else
            {
                // addition or subtraction may still need to be done
                if (operatorStack.Count() == 1 && isAddSub(operatorStack.Peek()) && operandStack.Count() == 2)
                {
                    String leftOperandString = operandStack.Pop();
                    String rightOperandString = operandStack.Pop();
                    String operatorString = operatorStack.Pop();
                    String calculation = calculateOperation(leftOperandString, rightOperandString, operatorString);

                    return int.Parse(calculation);
                }

                throw new ArgumentException("Error after processing, operator stack has value but" +
                    "it is not addition/subtraction and there is no two operands on the operand stack");
            }
        }

        /// <summary>
        /// Helper method for processing each token in the expression with the stacks.
        /// </summary>
        /// <param name="token">Token to process</param>
        /// <param name="operatorStack">Operator stack to use</param>
        /// <param name="operandStack">Operand stack to use</param>
        /// <param name="variableEvaluator">Variable evaluator to use</param>
        private static void processToken(String token, Stack<String> operatorStack, Stack<String> operandStack, Lookup variableEvaluator)
        {
            // skip empty tokens
            if (string.IsNullOrWhiteSpace(token))
            {
                return;
            }

            // process integers
            if (isInteger(token))
            {
                // if next operand is * or /
                if (isMultDiv(peekOrEmptyString(operatorStack)))
                {
                    // pop next operand and throw exception if empty
                    String operandStr = popOrEmptyString(operandStack);
                    if (operandStr.Equals(""))
                    {
                        throw new ArgumentException("operand stack empty when processing integer token");
                    }

                    // assignment says there should not be errors here
                    String operatorStr = operatorStack.Pop();
                    // apply operator to operands and push
                    operandStack.Push(calculateOperation(token, operandStr, operatorStr));
                }
                else
                {
                    operandStack.Push(token);
                }

                return;
            }

            // process variables
            if (isVariable(token))
            {
                // dereference variable
                String variableVal = variableEvaluator(token).ToString();

                // if top operator is * or /
                if (isMultDiv(peekOrEmptyString(operatorStack)))
                {
                    // pop next operand or throw exception if empty
                    String operandStr = popOrEmptyString(operandStack);
                    if (operandStr.Equals(""))
                    {
                        throw new ArgumentException("operand stack empty when processing integer token");
                    }

                    // asssigment says no error could occur here
                    String operatorStr = operatorStack.Pop();
                    // apply operator to operands and push
                    operandStack.Push(calculateOperation(variableVal, operandStr, operatorStr));
                }
                else
                {
                    operandStack.Push(variableVal);
                }

                return;
            }

            // process + and - operators
            if (isAddSub(token))
            {
                // if top operand is also + or -
                if (isAddSub(peekOrEmptyString(operatorStack)))
                {
                    // pop next two operands or throw exception if <2 operands on stack
                    String rightOperandStr = popOrEmptyString(operandStack);
                    String leftOperandStr = popOrEmptyString(operandStack);
                    if (rightOperandStr.Equals("") || leftOperandStr.Equals(""))
                    {
                        throw new ArgumentException("operand stack empty when processing integer token");
                    }

                    // assignment says no exception could occur here
                    String operatorStr = operatorStack.Pop();
                    // apply operator to operands and push
                    operandStack.Push(calculateOperation(rightOperandStr, leftOperandStr, operatorStr));

                    operatorStack.Push(token);
                }
                else
                {
                    operatorStack.Push(token);
                }

                return;
            }

            // process * and / operators
            if (isMultDiv(token))
            {
                operatorStack.Push(token);

                return;
            }

            // process ) token
            if (isLeftParenthesis(token))
            {
                operatorStack.Push(token);

                return;
            }

            if (isRightParenthesis(token))
            {
                // if + or - is on top of the operator stack
                if (isAddSub(peekOrEmptyString(operatorStack)))
                {
                    // pop next two operands or throw exception if <2 operands on stack
                    String rightOperandStr = popOrEmptyString(operandStack);
                    String leftOperandStr = popOrEmptyString(operandStack);
                    if (rightOperandStr.Equals("") || leftOperandStr.Equals(""))
                    {
                        throw new ArgumentException("operand stack empty when processing integer token");
                    }

                    // assignment says no error could occur here
                    String operatorStr = operatorStack.Pop();
                    operandStack.Push(calculateOperation(rightOperandStr, leftOperandStr, operatorStr));

                    // top of operator stack should be (, pop it
                    operatorStr = popOrEmptyString(operatorStack);
                    if (!operatorStr.Equals("("))
                    {
                        throw new ArgumentException("( not found at top of operator stack when ) processed.");
                    }
                }

                // if top of operator stack is * or /
                if (isMultDiv(peekOrEmptyString(operatorStack)))
                {
                    // pop next two operands or throw exception if <2 operands on stack
                    String rightOperandStr = popOrEmptyString(operandStack);
                    String leftOperandStr = popOrEmptyString(operandStack);
                    if (rightOperandStr.Equals("") || leftOperandStr.Equals(""))
                    {
                        throw new ArgumentException("operand stack empty when processing integer token");
                    }

                    // assignment says no error could occur here
                    String operatorStr = operatorStack.Pop();
                    // apply operator to operands and push
                    operandStack.Push(calculateOperation(rightOperandStr, leftOperandStr, operatorStr));
                }

                return;
            }

            // this point is reached if token is none of the above, in which case it is illegal
            throw new ArgumentException("Illegal token");
        }

        private static Boolean isEmpty(Stack<String> stack)
        {
            return (stack.Count() == 0);
        }

        /// <summary>
        /// Safely peek a stack by returning an empty string if it is empty.
        /// </summary>
        /// <param name="stack">Stack to peek</param>
        /// <returns>Top element of stack or empty string if stack is empty</returns>
        private static String peekOrEmptyString(Stack<String> stack)
        {
            try
            {
                String retVal = stack.Peek();
                return retVal;
            }
            catch (InvalidOperationException)
            {
                return "";
            }
        }

        /// <summary>
        /// Safely pop a stack by returning an empty string if it is empty.
        /// </summary>
        /// <param name="stack">Stack to pop</param>
        /// <returns>Top element of stack or empty string if stack is empty</returns>
        private static String popOrEmptyString(Stack<String> stack)
        {
            try
            {
                String retVal = stack.Pop();
                return retVal;
            }
            catch (InvalidOperationException)
            {
                return "";
            }
        }

        /// <summary>
        /// Determine if a token is an integer
        /// </summary>
        /// <param name="token">Token to evaluate</param>
        /// <returns></returns>
        private static Boolean isInteger(String token)
        {
            return int.TryParse(token, out int placeholder);
        }

        /// <summary>
        /// Determine if a token is a variable.
        /// </summary>
        /// <param name="token">Token to evaluate</param>
        /// <returns></returns>
        private static Boolean isVariable(String token)
        {
            // this regex pattern matches strings which begin with one more
            // more (uppercase or lowercase) letters, and end with one or more
            // decimal digits. This is the definition of variables on the assignment.
            String regex = "^[a-zA-Z]+\\d+$";

            return Regex.IsMatch(token, regex);
        }

        /// <summary>
        /// Determine if a token is an addition or subtraction operator.
        /// </summary>
        /// <param name="token">Token to evaluate</param>
        /// <returns></returns>
        private static Boolean isAddSub(String token)
        {
            return (token.Equals("-") || token.Equals("+"));
        }

        /// <summary>
        /// Determine if a token is a multiplication or division operator.
        /// </summary>
        /// <param name="token">Token to evaluate</param>
        /// <returns></returns>
        private static Boolean isMultDiv(String token)
        {
            return (token.Equals("*") || token.Equals("/"));
        }

        /// <summary>
        /// Determine if a token is a left parenthesis character.
        /// </summary>
        /// <param name="token">Token to evaluate</param>
        /// <returns></returns>
        private static Boolean isLeftParenthesis(String token)
        {
            return (token.Equals("("));
        }

        /// <summary>
        /// Determine if a token is a right parenthesis character.
        /// </summary>
        /// <param name="token">Token to evaluate</param>
        /// <returns></returns>
        private static Boolean isRightParenthesis(String token)
        {
            return (token.Equals(")"));
        }
        
        /// <summary>
        /// Compute the result of applying an operator to two operands
        /// </summary>
        /// <param name="rightOperand">Right operand</param>
        /// <param name="leftOperand">Left operand</param>
        /// <param name="operatorStr">Operator to evalutate operands with</param>
        /// <returns>Evaluation of *rightOperand* *operatorStr* *leftOperand*</returns>
        private static String calculateOperation(String rightOperand, String leftOperand, String operatorStr)
        {
            int right = int.Parse(rightOperand);
            int left = int.Parse(leftOperand);

            switch(operatorStr)
            {
                case "+":
                    return (left + right).ToString();
                case "-":
                    return (left - right).ToString();
                case "*":
                    return (left * right).ToString();
                case "/":
                    try
                    {
                        int retVal = (left / right);
                        return retVal.ToString();
                    }
                    catch(DivideByZeroException)
                    {
                        throw new ArgumentException("Division by zero occurred");
                    }
            }

            throw new ArgumentException("Illegal operator");
        }
    }
}