package ethics;

import ethics.Scenario.Actor;
import ethics.Scenario.Actor.Attribute;
import ethics.Scenario.Lane;

public class EthicsLab {
	/**
	 * Given a scenario similar to the ones found in The Moral Machine
	 * calculate the 'cost' of the scenario. The self-driving car
	 * will choose a scenario that 'costs' the  least. 
	 * @param scenario
	 * @return
	 */
	public boolean shouldSwitch(Scenario scenario) {return (weight(scenario.currentLane) > weight(scenario.oppositeLane));
	}
	
	/**
	 * Count the "weight" of the occupants of the lane.
	 * 
	 * Lane weights are the sum of the weights of the occupants.
	 * The weights of the occupants are as follows:
	 *  - Baby: 8
	 *  - Child: 7
	 *  - Pregnant: 6
	 *  - Executives: 5
	 *  - Doctors: 4
	 *  - Males, Females, Adult, Athletic: 3
	 *  - Homeless, Elder, Criminal, Overweight: 2
	 *  - Cats, Dogs: 1
	 * 
	 * @param lane
	 * @return
	 */
	private double weight(Lane lane) {
		double cost = 0.0;
		
		for (Actor actor: lane.actors) {
			for (Attribute attribute: actor.getAttributes()) {
				switch (attribute) {
				case BABY: cost += 8; break;
				case CHILD: cost += 7; break;
				case PREGNANT: cost += 6; break;
				case EXECUTIVE: cost += 5; break;
				case DOCTOR: cost += 4; break;
				case MALE: cost += 3; break;
				case FEMALE: cost += 3; break;
				case ADULT: cost += 3; break;
				case ATHLETIC: cost += 3; break;
				case HOMELESS: cost += 2; break;
				case ELDER: cost += 2; break;
				case CRIMINAL: cost += 2; break;
				case OVERWEIGHT: cost += 2; break;
				case CAT: cost += 1; break;
				case DOG: cost += 1; break;
				}
			}
		}
		
		return cost;
	}
	
	public static void main(String[] args) {
		new Simulator().runSimulation(100_000);
	}
}
