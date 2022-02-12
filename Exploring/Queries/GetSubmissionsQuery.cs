using System;
using System.Collections.Generic;
using MediatR;
using Types.Chats;
using Types.Exploring;

namespace Exploring.Queries
{
    public class GetSubmissionsQuery : IRequest<IEnumerable<Submission>> { }
}