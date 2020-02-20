package ethics;

import java.util.ArrayList;
import java.util.List;
import java.util.Random;

import ethics.Scenario.Actor;
import ethics.Scenario.Bystander;
import ethics.Scenario.Lane;
import ethics.Scenario.Outcome;

public class ScenarioGenerator {
	private Random rng;

	public List<Scenario> generateScenarios(int number) {
		rng = new Random(2420);
		List<Scenario> scenarios = new ArrayList<>();
		for(int count = 0 ; count < number; count++) {
			scenarios.add(generateScenario());
		}
		return scenarios;
	}

	private Scenario generateScenario() {
		Scenario scenario = new Scenario();
		scenario.currentLane = generateLane();
		scenario.oppositeLane = generateLane(); 
		return scenario;
	}

	private Lane generateLane() {
		Lane lane = new Lane();
		lane.illegalCrossing = rng.nextBoolean();
		lane.outcome = getOutcome();
		lane.actors = generateActors();
		lane.bystander = passengerOrPedestrian();
		return lane;
	}

	private Bystander passengerOrPedestrian() {
		return rng.nextBoolean() ? Bystander.PASSENGER : Bystander.PEDESTRIAN;
	}

	private List<Actor> generateActors() {
		int number = 1 + rng.nextInt(4);
		List<Actor> actors = new ArrayList<>();
		for(int count = 0; count < number; count++) {
			actors.add(generateActor());
		}
		return actors;
	}

	private Actor generateActor() {
		double probability = rng.nextDouble();
		double distribution = (1.0 / 27.0);
		if(probability < distribution) {
			return Actor.man();
		} else if (probability < distribution * 2) {
			return Actor.woman();
		} else if (probability < distribution * 3) {
			return Actor.boy();
		} else if (probability < distribution * 4) {
			return Actor.girl();
		} else if (probability < distribution * 5) {
			return Actor.elderMan();
		} else if (probability < distribution * 6) {
			return Actor.elderWoman();
		} else if (probability < distribution * 7) {
			return Actor.overweightMan();
		} else if (probability < distribution * 8) {
			return Actor.overweightWoman();
		} else if (probability < distribution * 9) {
			return Actor.maleExecutive();
		} else if (probability < distribution * 10) {
			return Actor.femaleExecutive();
		} else if (probability < distribution * 11) {
			return Actor.maleDoctor();
		} else if (probability < distribution * 12) {
			return Actor.femaleDoctor();
		} else if (probability < distribution * 13) {
			return Actor.athleticMale();
		} else if (probability < distribution * 14) {
			return Actor.athleticFemale();
		} else if (probability < distribution * 15) {
			return Actor.pregnantWoman();
		} else if (probability < distribution * 16) {
			return Actor.criminalMan();
		} else if (probability < distribution * 17) {
			return Actor.criminalWoman();
		} else if (probability < distribution * 18) {
			return Actor.criminalAdult();
		} else if (probability < distribution * 19) {
			return Actor.criminalChild();
		} else if (probability < distribution * 20) {
			return Actor.homelessMan();
		} else if (probability < distribution * 21) {
			return Actor.homelessWoman();
		} else if (probability < distribution * 22) {
			return Actor.homelessChild();
		} else if (probability < distribution * 23) {
			return Actor.homelessAdult();
		} else if (probability < distribution * 24) {
			return Actor.babyBoy();
		} else if (probability < distribution * 25) {
			return Actor.babyGirl();
		} else if (probability < distribution * 26) {
			return Actor.cat();
		} else  {
			return Actor.dog(); 
		}
	}
	
	private Outcome getOutcome() {
		double probability = rng.nextDouble();
		if(probability < .3333) {
			return Outcome.INJURED;
		} else if (probability < .6666) {
			return Outcome.KILLED;
		} else  {
			return Outcome.UNCERTAIN;
		}
	}
}
