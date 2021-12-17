class Simulation
{
    private ((int xmin, int xmax) x, (int ymin, int ymax) y) target;
    public Simulation(((int xmin, int xmax) x, (int ymin, int ymax) y) tg)
    {
        target = tg;
    }

    private (bool hit, int x, int y, int y_max) Simulate(int dx, int dy)
    {
        bool hit = false;
        var x = 0;
        var y = 0;
        var y_max = int.MinValue;

        while (true)
        {
            x += dx;
            y += dy;
            dx -= dx > 0 ? 1 : dx < 0 ? -1 : 0;
            dy -= 1;
            y_max = (y > y_max) switch
            {
                true => y,
                false => y_max
            };
            if (x >= target.x.xmin &&
                x <= target.x.xmax &&
                y >= target.y.ymin &&
                y <= target.y.ymax)
            {
                hit = true;
                break;
            }
            if (y < target.y.ymin)
            {
                break;
            }
        }
        return (hit, x, y, y_max);
    }

    public IEnumerable<(bool hit, int x, int y, int y_max)> RunSim()
    {
        foreach (var dx in Enumerable.Range(-1000, 2000))
            foreach (var dy in Enumerable.Range(-1000, 2000))
            {
                var r = Simulate(dx, dy);
                if (r.hit)
                {
                    yield return r;
                }
            }
    }
}