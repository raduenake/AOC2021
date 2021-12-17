class Simulation
{
    private ((int xMin, int xMax) X, (int yMin, int yMax) Y) targetArea;
    public Simulation(((int xMin, int xMax) X, (int yMin, int yMax) Y) _target)
    {
        targetArea = _target;
    }

    private (bool hit, int x, int y, int yMaxHit) Simulate(int dx, int dy)
    {
        bool hit = false;
        var x = 0;
        var y = 0;
        var yMaxHit = int.MinValue;

        while (true)
        {
            x += dx;
            y += dy;
            dx -= dx > 0 ? 1 : dx < 0 ? -1 : 0;
            dy -= 1;
            yMaxHit = (y > yMaxHit) switch
            {
                true => y,
                false => yMaxHit
            };
            if (x >= targetArea.X.xMin &&
                x <= targetArea.X.xMax &&
                y >= targetArea.Y.yMin &&
                y <= targetArea.Y.yMax)
            {
                hit = true;
                break;
            }
            if (y < targetArea.Y.yMin)
            {
                break;
            }
        }
        return (hit, x, y, yMaxHit);
    }

    public IEnumerable<(bool hit, int x, int y, int yMaxHit)> RunSim()
    {
        foreach (var dy in Enumerable.Range(-150, 250))
            foreach (var dx in Enumerable.Range(0, 250))
            {
                var result = Simulate(dx, dy);
                if (result.hit)
                {
                    yield return result;
                }
            }
    }
}