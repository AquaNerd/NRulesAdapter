using System.Collections.Generic;
using System.Threading.Tasks;

namespace RulesRunner {
    public interface IRulesAdapter {
        void Insert(IEnumerable<object> facts);
        void Update(IEnumerable<object> facts);
        Task<int> Fire();
    }
}