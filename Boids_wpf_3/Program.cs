Game.Init();

var watch = System.Diagnostics.Stopwatch.StartNew();

for (int i = 0; i < 2000; i++) Game.Update(false);

// for (int i = 0; i < 19; i++) Game.Update(false);
// for (int i = 0; i < 1; i++) Game.Update(true);

watch.Stop();
System.Console.WriteLine(watch.ElapsedMilliseconds);

Game.End();

/*
C0ABCC8BC113B664
411287424128493F
-------
C0ABCC8BC113B664
4110C3F741259639
*/