using System;
using System.Collections.Generic;
using System.Linq;
using Google.Cloud.Firestore;

namespace Types.Other
{
    public class GuidConverter : IFirestoreConverter<Guid>, IFirestoreConverter<List<Guid>>
    {
        public object ToFirestore(Guid value) => value.ToString("D");
        public object ToFirestore(List<Guid> value) => value.Select(guid => guid.ToString("D"));
        
        List<Guid> IFirestoreConverter<List<Guid>>.FromFirestore(object value)
        {
            return value switch
            {
                List<object> guids => guids.Select(guid => Guid.ParseExact(guid.ToString(), "D")).ToList(),
                null => throw new ArgumentNullException(nameof(value)),
                _ => throw new ArgumentException($"Unexpected data: {value.GetType()}")
            };
        }

        Guid IFirestoreConverter<Guid>.FromFirestore(object value)
        {
            return value switch
            {
                string guid => Guid.ParseExact(guid, "D"),
                null => throw new ArgumentNullException(nameof(value)),
                _ => throw new ArgumentException($"Unexpected data: {value.GetType()}")
            };
        }
    }
}