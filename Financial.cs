namespace Depot;

public class Financial
    {
        private const double tol = 0.001;

        public static double RateOfReturn(IEnumerable<Transaction> transactions)
        {
            var baseT = transactions
                .Select(t => (t.Amount, (double)(t.Date - DateTime.MinValue).TotalDays))
                .ToArray();

            var x0 = 0.1;
            var x1 = 0.0;
            var diff = 1e+100;

            var fx = total_f_xirr(baseT);
            var fdx = total_df_xirr(baseT);

            while (diff > tol) 
            {
                x1 = x0 - fx(x0) / fdx(x0);
                diff = Math.Abs(x1 - x0);
                x0 = x1;
            }

            return x0;
        }

        private static Func<double, double> total_f_xirr((double payment, double time)[] transactions) 
        {
            var resf = (double x) => 0.0;

            for (int i = 0; i < transactions.Length; i++) 
            {
                resf = composeFunctions(resf, f_xirr(transactions[i].payment, transactions[i].time - transactions[0].time));
            }

            return resf;
        }

        private static Func<double, double> total_df_xirr((double payment, double time)[] transactions) 
        {
            var resf = (double x) => 0.0;

            for (var i = 0; i < transactions.Length; i++) 
            {
                resf = composeFunctions(resf, df_xirr(transactions[i].payment, transactions[i].time - transactions[0].time));
            }

            return resf;
        }

        private static Func<double, double> composeFunctions(Func<double, double> f1, Func<double, double> f2) 
        {
            return x => f1(x) + f2(x);
        }

        private static Func<double, double> f_xirr(double p, double dt) 
        {
            return x => p * Math.Pow((1.0 + x), (-dt / 365.0));
        }

        private static Func<double, double> df_xirr(double p, double dt) 
        {
            return x => (1.0 / 365.0) * -dt * p * Math.Pow((x + 1.0), ((-dt / 365.0) - 1.0));
        }
    } 