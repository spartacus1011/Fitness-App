using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_tracker.Classes
{
    public class ExerciseHistoryItem //I think this way of implementing things would be very difficult to work
    {
        //yeah, this might not be needed at all!
        public int TotalCount { get; set; }
        public List<DateTime> CompleteDateTimes { get; set; }

        public string GUIDID { get { return guidid; } } //this is probably not needed at all
        private readonly string guidid;

        public ExerciseHistoryItem(string guidid) //i dont like putting something in the constructor but it might be the only way. This would work fine if i do this as a database...
        {
            this.guidid = guidid;
        }

        public void CompleteExercise()
        {
            TotalCount++;
            CompleteDateTimes.Add(DateTime.Now);
        }

        public void CompleteRep()
        {

        }

    }
}
