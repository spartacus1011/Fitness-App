
namespace Exercise_tracker.Helpers
{
    public enum ExercisetypeEnum
    {
        SingleReps,
        NormalSets,
        Timed,
        TimedSets
    }

    public enum ExerciseTimeUnitsEnum
    {
        Seconds,
        Minutes,
        Hours
    }

    public enum ExerciseRecurrenceEnum
    {
        Daily,
        Weekly,
        Monthly
        //hourly //doing this would require a change to the way the count down timer works. Not really worth it yet
    }

    public enum ExerciseRestTimeEnum
    {
        Seconds30,
        Seconds60,
        Seconds90,
        Seconds120
    }

    public enum MuscleGroupEnum
    {
        Biceps,
        Triceps,
        Forearms,
        Chest,
        Back,
        Abs,
        Calves,
        Hamstrings,
        Quads,
        Cardio, //Lungs are a muscle group too
        Other,
    }

}
