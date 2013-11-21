using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FiboThreading {
	class Program {
		static void Main(string[] args) {

			const int FibonacciCalculations = 50;
			System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
			// One event is used for each Fibonacci object
			ManualResetEvent[] doneEvents = new ManualResetEvent[FibonacciCalculations];
			Fibonacci[] fibArray = new Fibonacci[FibonacciCalculations];
			Random r = new Random();

			watch.Start();

			// Configure and launch threads using ThreadPool:
			Console.WriteLine("launching {0} tasks...", FibonacciCalculations);
			for (int i = 0; i < FibonacciCalculations; i++) {
				doneEvents[i] = new ManualResetEvent(false);
				//Fibonacci f = new Fibonacci(r.Next(20, 40), doneEvents[i]);
				Fibonacci f = new Fibonacci(i, doneEvents[i]);
				fibArray[i] = f;
				ThreadPool.QueueUserWorkItem(f.ThreadPoolCallback, i);
			}

			// Wait for all threads in pool to calculation...
			WaitHandle.WaitAll(doneEvents);
			Console.WriteLine("All calculations are complete.");
			watch.Stop();
			// Display the results...
			for (int i = 0; i < FibonacciCalculations; i++) {
				Fibonacci f = fibArray[i];
				Console.WriteLine("Fibonacci({0}) = {1}", f.N, f.FibOfN);
			}
			Console.WriteLine("Calculations were done in {0} seconds ", watch.Elapsed.TotalSeconds);
			Console.ReadKey();
		}
	}
}
