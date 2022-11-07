Game.Init();

var watch = System.Diagnostics.Stopwatch.StartNew();

// for (int i = 0; i < 18; i++) Game.Update(false);
// for (int i = 0; i < 1; i++) Game.Update(true);
for (int i = 0; i < 1000; i++) Game.Update(false);

watch.Stop();
System.Console.WriteLine(watch.ElapsedMilliseconds);

Game.End();