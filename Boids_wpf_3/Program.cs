Game.Init();

var watch = System.Diagnostics.Stopwatch.StartNew();

for (int i = 0; i < 50; i++) Game.Update(false);

watch.Stop();
// System.Console.WriteLine(watch.ElapsedMilliseconds);

Game.End();