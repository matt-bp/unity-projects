using Unity.Mathematics;

namespace MattMath
{
    public static partial class mm
    {
        public static double2x2 outerProduct(double2 column, double2 row)
        {
            var result = double2x2.zero;

            result[0][0] = column.x * row.x;
            result[0][1] = column.y * row.x;

            result[1][0] = column.x * row.y;
            result[1][1] = column.y * row.y;
            
            return double2x2.identity;
        }
    }
}