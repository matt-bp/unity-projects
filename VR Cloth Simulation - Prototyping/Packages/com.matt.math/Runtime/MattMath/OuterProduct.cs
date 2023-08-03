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
            
            return result;
        }
        
        public static double3x3 outerProduct(double3 column, double3 row)
        {
            var result = double3x3.zero;

            result[0][0] = column.x * row.x;
            result[0][1] = column.y * row.x;
            result[0][2] = column.z * row.x;

            result[1][0] = column.x * row.y;
            result[1][1] = column.y * row.y;
            result[1][2] = column.z * row.y;
            
            result[2][0] = column.x * row.z;
            result[2][1] = column.y * row.z;
            result[2][2] = column.z * row.z;
            
            return result;
        }
    }
}