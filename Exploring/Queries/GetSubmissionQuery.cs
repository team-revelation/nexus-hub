using System;
using Google.Cloud.Firestore;
using MediatR;
using Types.Chats;
using Types.Exploring;

namespace Exploring.Queries
{
    public class GetSubmissionQuery : IRequest<Submission>
    {
        public Guid Uuid { get; }

        public GetSubmissionQuery(Guid uuid)
        {
            Uuid = uuid;
        }
    }
}