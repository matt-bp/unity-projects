using Conditions;
using Unity.Mathematics;

namespace Forces
{
    public class ConditionForces
    {
        private readonly IConditionQuantities cq;
        private readonly double k;
        private readonly double kd;
        
        public ConditionForces(double inK, double inKd, IConditionQuantities conditionQuantities)
        {
            cq = conditionQuantities;
            k = inK;
            kd = inKd;
        }

        /// <summary>
        /// Getting the force from this condition.
        ///
        /// Check equation 7.2 (pg. 60) for general form.
        /// </summary>
        /// <param name="i">Particle to consider</param>
        /// <returns></returns>
        public double3 GetForce(int i) => -k * (cq.Dcu[i] * cq.Cu + cq.Dcv[i] * cq.Cv);

        /// <summary>
        /// Gets the damping force for this condition;
        ///
        /// Check equation 7.5 (pg. 61) for the general form.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double3 GetDampingForce(int i) => -kd * (cq.Dcu[i] * cq.CuDot + cq.Dcv[i] * cq.CvDot);
    }
}