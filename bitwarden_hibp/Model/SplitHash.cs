using System;
using System.Collections.Generic;

namespace bitwarden_hibp.Model
{
    public class SplitHash
    {
        public string HashPrefix { get; private set;}

        public string HashSuffix { get; private set; }
        
        public string Hash {             
            get { return _hash; } 
            set {
                this._hash = HashProvider.GetHashString(value);
                this.HashPrefix = _hash.Substring(0,5);
                this.HashSuffix = _hash.Substring(5);
            } 
        }
        private string _hash;

        public override int GetHashCode() {
            return this.Hash.GetHashCode();
        }

        public SplitHash() {}
        public SplitHash(string hash) {
            this.Hash = hash;
        }
    }
}
