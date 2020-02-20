package ethics;

import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import ethics.Scenario.Actor;
import ethics.Scenario.Actor.Attribute;
import ethics.Scenario.Bystander;
import ethics.Scenario.Lane;
import ethics.Scenario.Outcome;

public class Simulator {
	private static final String CENT_FORMAT = "%5.02f%%";
	private Map<Attribute, Integer> totalAttrs;
	private Map<Attribute, Integer> collided;
	private Map<Attribute, Integer> avoided;
	private Map<Bystander, Integer> bystanders;
	private Map<Outcome, Integer> outcomes;
	private int timesSwitched;
	private int numberOfSimulations;
	private int lessPopulatedLane;
	private Map<OutcomeHolder, Integer> outcomeHolders;
	
	public void runSimulation(int numberOfSimulations) {
		initMaps();
		this.numberOfSimulations = numberOfSimulations;
		List<Scenario> scenarios = new ScenarioGenerator().generateScenarios(numberOfSimulations);
		System.out.println("Running " + numberOfSimulations + " scenarios.");
		runScenarios(scenarios);
		System.out.println("Calculating...");
		printActorCount();
		printTimesSwitched();
		printBystanders();
		printPercentages();
		printOutcomes();
	}

	private void printOutcomes() {
		int total = outcomes.values().stream().mapToInt((count -> count)).sum();
		double uncertainPercentage = ((double)outcomes.get(Outcome.UNCERTAIN) / total) * 100.0; 
		double killedPercentage = ((double)outcomes.get(Outcome.KILLED) / total) * 100.0; 
		double injuredPercentage = ((double)outcomes.get(Outcome.INJURED) / total) * 100.0;
		
		System.out.println(String.format("The car chose a lane with an uncertain outcome " + CENT_FORMAT + " of the time, injured bystanders " 
				+ CENT_FORMAT + " of the time and killed bystanders " 
				+ CENT_FORMAT + " of the time.", uncertainPercentage, killedPercentage, injuredPercentage));
	}

	private void printActorCount() {
		System.out.println(String.format("The car chose a less populated lane " + CENT_FORMAT + " of the time.", ((double)lessPopulatedLane / numberOfSimulations) * 100.0));
	}

	private void printTimesSwitched() {
		System.out.println(String.format("The car chose to switch lanes %5.02f%% of the time.", ((double)timesSwitched / numberOfSimulations) * 100.0));
	}

	private void printBystanders() {
		int totalBystanders = bystanders.values().stream().mapToInt(count->count).sum();
		double percentPed = (bystanders.get(Bystander.PEDESTRIAN) / (double)totalBystanders) * 100.0;
		double percentPassenger = (bystanders.get(Bystander.PASSENGER) / (double)totalBystanders) * 100.0;
		System.out.println(String.format("%02.2f%% of the collisions were with pedestrians, the other %02.2f%% were with passengers.", percentPed, percentPassenger));
	}

	private void printPercentages() {
		List<Attribute> attributes = Arrays.asList(Attribute.values());
		attributes.sort((lhs, rhs) -> lhs.toString().compareTo(rhs.toString()));
		System.out.println("You collided with:");
		int idx;
		for(idx = 0; idx < attributes.size()-2; idx+=2) {
			Attribute left = attributes.get(idx);
			double leftPercentCollided = (collided.get(left) / (double)totalAttrs.get(left)) * 100.0;
			
			Attribute right = attributes.get(idx+1);
			double rightPercentCollided = (collided.get(right) / (double)totalAttrs.get(right)) * 100.0;
			System.out.println(String.format("%-10s %05.2f%% | %-10s %05.2f%%", left, leftPercentCollided, right, rightPercentCollided));
		}
		
		for(;idx < attributes.size(); idx++) {
			Attribute left = attributes.get(idx);
			double leftPercentCollided = (collided.get(left) / (double)totalAttrs.get(left)) * 100.0;
			System.out.println(String.format("%-10s %05.2f%%", left, leftPercentCollided));
		}
	}

	private void runScenarios(List<Scenario> scenarios) {
		for(Scenario scenario : scenarios ) {
			countAttributesIn(scenario);
			boolean switched = new EthicsLab().shouldSwitch(scenario);
			Lane collisionLane = switched ? scenario.oppositeLane : scenario.currentLane;
			collidedAndAvoided(scenario, switched);
			recordSwitches(switched);
			recordBystanders(scenario, switched);
			recordOutcome(scenario, collisionLane);
			recordLessPopulatedChoice(scenario, switched);
			recordOutcomeRelation(scenario, switched);
		}
	}

	private void recordOutcome(Scenario scenario, Lane collisionLane) {
		outcomes.computeIfPresent(collisionLane.outcome, (__, count) -> count + (collisionLane.actors.size()));
	}
	
	private void recordOutcomeRelation(Scenario scenario, boolean switched) {
		Outcome collided, avoided;
		if(switched) {
			collided = scenario.oppositeLane.outcome;
			avoided = scenario.currentLane.outcome;
		} else {
			avoided = scenario.oppositeLane.outcome;
			collided = scenario.currentLane.outcome;
		}
		outcomeHolders.computeIfPresent(new OutcomeHolder(collided, avoided), (__, count) -> count + 1);
	}

	private void recordLessPopulatedChoice(Scenario scenario, boolean switched) {
		boolean lessInOtherLane = scenario.currentLane.actors.size() > scenario.oppositeLane.actors.size();
		if((switched && lessInOtherLane) || (!switched && !lessInOtherLane)) {
			lessPopulatedLane++;
		}
	}

	private void recordSwitches(boolean switched) {
		if(switched) {
			timesSwitched++;
		}
	}

	private void recordBystanders(Scenario scenario, boolean switched) {
		Bystander bystander = switched ? scenario.oppositeLane.bystander : scenario.currentLane.bystander;
		bystanders.computeIfPresent(bystander, (__, count)  -> count+1);
	}

	private void collidedAndAvoided(Scenario scenario, boolean switched) {
		if(switched) {
			incrementCollided(scenario.oppositeLane);
			incrementAvoided(scenario.currentLane);
		} else {
			incrementCollided(scenario.currentLane);
			incrementAvoided(scenario.oppositeLane);
		}
	}

	private void incrementAvoided(Lane lane) {
		incrementAttributes(avoided, lane.actors);
	}

	private void incrementCollided(Lane lane) {
		incrementAttributes(collided, lane.actors);
	}

	private void countAttributesIn(Scenario scenario) {
		incrementAttributes(totalAttrs, scenario.currentLane.actors);
		incrementAttributes(totalAttrs, scenario.oppositeLane.actors);
	}
	
	private void incrementAttributes(Map<Attribute, Integer> map, List<Actor> actors) {
		for(Actor actor : actors) {
			for(Attribute attribute : actor.getAttributes()) {
				map.computeIfPresent(attribute, (__, count) -> count + 1);
			}
		}
	}

	private void initMaps() {
		totalAttrs = initAttributeMap();
		collided = initAttributeMap();
		avoided = initAttributeMap();
		bystanders = initBystanderMap();
		outcomes = initOutcomeMap();
		outcomeHolders = initOutcomeHolderMap();
	}

	private Map<Outcome, Integer> initOutcomeMap() {
		Map<Outcome, Integer> map = new HashMap<>();
		for(Outcome outcome : Outcome.values()) {
			map.put(outcome, 0);
		}
		return map;
	}
	
	private Map<OutcomeHolder, Integer> initOutcomeHolderMap() {
		Map<OutcomeHolder, Integer> map = new HashMap<>();
		map.put(new OutcomeHolder(Outcome.KILLED, Outcome.INJURED), 0);
		map.put(new OutcomeHolder(Outcome.KILLED, Outcome.UNCERTAIN), 0);
		map.put(new OutcomeHolder(Outcome.INJURED, Outcome.KILLED), 0);
		map.put(new OutcomeHolder(Outcome.INJURED, Outcome.UNCERTAIN), 0);
		map.put(new OutcomeHolder(Outcome.UNCERTAIN, Outcome.KILLED), 0);
		map.put(new OutcomeHolder(Outcome.UNCERTAIN, Outcome.INJURED), 0);
		return map;
	}

	private Map<Bystander, Integer> initBystanderMap() {
		Map<Bystander, Integer> map = new HashMap<>();
		for(Bystander bystander : Bystander.values()) {
			map.put(bystander, 0);
		}
		return map;
	}

	private Map<Attribute, Integer> initAttributeMap() {
		Map<Attribute, Integer> map = new HashMap<>();
		for(Attribute attribute : Attribute.values()) {
			map.put(attribute, 0);
		}
		return map;
	}
	
	private class OutcomeHolder {
		Outcome collided, avoided;
		
		public OutcomeHolder(Outcome collided, Outcome avoided) {
			this.collided = collided;
			this.avoided = avoided;
		}
		
		@Override
		public int hashCode() {
			return collided.toString().hashCode() + avoided.toString().hashCode();
		}
		
		@Override
		public boolean equals(Object obj) {
			if(obj instanceof OutcomeHolder) {
				return this.collided.equals(((OutcomeHolder) obj).collided) 
						&& this.avoided.equals(((OutcomeHolder) obj).avoided);
			}
			return false;
		}
	}
}
