using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NUnit.Framework.Constraints;

namespace CalisthenicsStore.Tests.ServiceTests.Other
{
    public class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> sessionStorage = new Dictionary<string, byte[]>();

        public bool IsAvailable => true;

        public string Id { get; } = Guid.NewGuid().ToString();

        public IEnumerable<string> Keys => sessionStorage.Keys;

        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        
        public bool TryGetValue(string key, [NotNullWhen(true)] out byte[]? value) => sessionStorage.TryGetValue(key, out value);

        public void Set(string key, byte[] value) => sessionStorage[key] = value;

        public void Remove(string key) => sessionStorage.Remove(key);

        public void Clear() => sessionStorage.Clear();

    }
}
