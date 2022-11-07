Game.Init();

var watch = System.Diagnostics.Stopwatch.StartNew();

for (int i = 0; i < 18; i++) Game.Update();

watch.Stop();
System.Console.WriteLine(watch.ElapsedMilliseconds);

Game.End();