using Conditions;
using Unity.Mathematics;

namespace Forces
{
    public interface IConditionForceCalculator4
    {
        /// <summary>
        /// Getting the force from this condition.
        ///
        /// Check equation 7.2 (pg. 60) for general form.
        /// </summary>
        /// <param name="i">Particle to consider</param>
        /// <returns></returns>
        public double3 GetForce(int i);

        /// <summary>
        /// Gets the damping force for this condition;
        ///
        /// Check equation 7.5 (pg. 61) for the general form.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double3 GetDampingForce(int i);

        /// <summary>
        /// Gets the forces first partial derivative with respect to each particle.
        ///
        /// Check out equation 7.3 (pg. 60) for the general form.
        /// </summary>
        /// <param name="i">The particle we are taking the derivative with respect to.</param>
        /// <returns></returns>
        public WithRespectTo4<double3x3> GetForceFirstPartialDerivative(int i);

        /// <summary>
        /// Gets the damping force partial derivative with respect to the positions of each of the particles.
        ///
        /// Check out equation 7.8 (pg. 62) for the general form.
        ///
        /// The second bit, \partial C(x) / \partial x_j ^T vj is the same as C dot. 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public WithRespectTo4<double3x3> GetDampingForcePartialDerivativeWrtPosition(int i);

        /// <summary>
        /// Gets the damping force partial derivative with respect to the velocities of the particles.
        ///
        /// Check out equation 7.11 (pg. 62) for the general form.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public WithRespectToV4<double3x3> GetDampingForcePartialDerivativeWrtVelocity(int i);
    }
}