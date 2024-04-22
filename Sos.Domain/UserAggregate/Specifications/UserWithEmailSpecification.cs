using Sos.Domain.Core.Commons.Bases;
using Sos.Domain.UserAggregate.ValueObjects;
using System.Linq.Expressions;

namespace Sos.Domain.UserAggregate.Specifications
{
    /// <summary>
    /// Respresents the specification for the user with email.
    /// </summary>
    public sealed class UserWithEmailSpecification : Specification<User>
    {
        private readonly Email _email;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserWithEmailSpecification"/> class.
        /// </summary>
        /// <param name="email">The email.</param>
        public UserWithEmailSpecification(Email email) => _email = email;

        /// <inheritdoc />
        internal override Expression<Func<User, bool>> ToExpression()
            => user => user.Email!.Value == _email;
    }
}
