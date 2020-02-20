package ethics;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;

public class Scenario {
	Lane currentLane;
	Lane oppositeLane;
	
	public static class Lane { 
		public List<Actor> actors;
		public boolean illegalCrossing;
		public Outcome outcome;
		public Bystander bystander;
	}
	
	public enum Outcome {
		KILLED, INJURED, UNCERTAIN
	}
	
	public enum Bystander {
		PEDESTRIAN, PASSENGER
	}
	
	public static class Actor {
		public static enum Attribute {
			DOCTOR, EXECUTIVE, PREGNANT, 
			MALE, FEMALE, CAT, DOG,
			BABY, HOMELESS, CRIMINAL, OVERWEIGHT,
			ADULT, CHILD, ELDER, ATHLETIC
		}
		
		private List<Attribute> attributes;
		
		public Actor(List<Attribute> attributes) {
			this.attributes = attributes;
		}
		
		public Actor(Actor actor) {
			this.attributes = Collections.unmodifiableList(new ArrayList<>(attributes));
		}
		
		public boolean is(Attribute attribute) {
			return attributes.contains(attribute);
		}
		
		public List<Attribute> getAttributes() {
			return Collections.unmodifiableList(attributes);
		}
		public void addAttribute(Attribute attribute) {
			ArrayList<Attribute> newAttributes = new ArrayList<>(attributes);
			newAttributes.add(attribute);
			this.attributes = Collections.unmodifiableList(newAttributes);
		}

		public static Actor man() {
			return new Actor(Arrays.asList(Attribute.MALE, Attribute.ADULT));
		}
		
		public static Actor woman() {
			return new Actor(Arrays.asList(Attribute.FEMALE, Attribute.ADULT));
		}
		
		public static Actor boy() {
			return new Actor(Arrays.asList(Attribute.MALE, Attribute.CHILD));
		}
		
		public static Actor girl() {
			return new Actor(Arrays.asList(Attribute.FEMALE, Attribute.CHILD));
		}
		
		public static Actor elderMan() {
			return new Actor(Arrays.asList(Attribute.MALE, Attribute.ELDER));
		}
		
		public static Actor elderWoman() {
			return new Actor(Arrays.asList(Attribute.FEMALE, Attribute.ELDER));
		}
		
		public static Actor overweightMan() {
			return new Actor(Arrays.asList(Attribute.MALE, Attribute.OVERWEIGHT));
		}
		
		public static Actor overweightWoman() {
			return new Actor(Arrays.asList(Attribute.FEMALE, Attribute.OVERWEIGHT));
		}
		
		public static Actor maleExecutive() {
			return new Actor(Arrays.asList(Attribute.MALE, Attribute.EXECUTIVE));
		}
		
		public static Actor femaleExecutive() {
			return new Actor(Arrays.asList(Attribute.FEMALE, Attribute.EXECUTIVE));
		}
		
		public static Actor maleDoctor() {
			return new Actor(Arrays.asList(Attribute.MALE, Attribute.DOCTOR));
		}
		
		public static Actor femaleDoctor() {
			return new Actor(Arrays.asList(Attribute.FEMALE, Attribute.DOCTOR));
		}
		
		public static Actor athleticMale() {
			return new Actor(Arrays.asList(Attribute.MALE, Attribute.ATHLETIC));
		}
		
		public static Actor athleticFemale() {
			return new Actor(Arrays.asList(Attribute.FEMALE, Attribute.ATHLETIC));
		}
		
		public static Actor pregnantWoman() {
			return new Actor(Arrays.asList(Attribute.FEMALE, Attribute.PREGNANT));
		}
		
		public static Actor criminalMan() {
			return new Actor(Arrays.asList(Attribute.MALE, Attribute.CRIMINAL));
		}
		
		public static Actor criminalWoman() {
			return new Actor(Arrays.asList(Attribute.FEMALE, Attribute.CRIMINAL));
		}
		
		public static Actor criminalAdult() {
			return new Actor(Arrays.asList(Attribute.CRIMINAL, Attribute.ADULT));
		}
		
		public static Actor criminalChild() {
			return new Actor(Arrays.asList(Attribute.CRIMINAL, Attribute.CHILD));
		}
		
		public static Actor homelessMan() {
			return new Actor(Arrays.asList(Attribute.HOMELESS, Attribute.MALE));
		}
		
		public static Actor homelessWoman() {
			return new Actor(Arrays.asList(Attribute.HOMELESS, Attribute.FEMALE));
		}
		
		public static Actor homelessChild() {
			return new Actor(Arrays.asList(Attribute.HOMELESS, Attribute.CHILD));
		}
		
		public static Actor homelessAdult() {
			return new Actor(Arrays.asList(Attribute.HOMELESS, Attribute.ADULT));
		}
		
		public static Actor babyBoy() {
			return new Actor(Arrays.asList(Attribute.MALE, Attribute.BABY));
		}
		
		public static Actor babyGirl() {
			return new Actor(Arrays.asList(Attribute.FEMALE, Attribute.BABY));
		}
		
		public static Actor cat() {
			return new Actor(Arrays.asList(Attribute.CAT));
		}
		
		public static Actor dog() {
			return new Actor(Arrays.asList(Attribute.DOG));
		}
	}
}
