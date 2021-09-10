using System;
using System.Collections.Generic;

public class LruCache<TKey, TValue> 
{
    class LruNode 
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public LruNode Prev { get; set; } = null;
        public LruNode Next { get; set; } = null;

        public LruNode(TKey key, TValue value) 
        {
            this.Key = key;
            this.Value = value;
        }
    }

    private Dictionary<TKey, LruNode> map = null;
    private int counter = 0;
    private readonly int capacity = 10;
    private LruNode Head, Last = null;

    public LruCache(int capacity) 
    {
        this.capacity = capacity;
        map = new Dictionary<TKey, LruNode>(capacity);
    }

    public bool Exists(TKey key) => map.ContainsKey(key);

    public int Count { get { return counter; } }

    public LruNode Get(TKey key) 
    {
        if (!Exists(key))
            throw new KeyNotFoundException($"Key not found.");

        var node = map[key];
        Remove(node);
        AddToLast(node);
        return node;
    }

    public void Set(TKey key, TValue value) 
    {
        var node = new LruNode(key, value);

        if (counter == 0) {
            Head = node;
        }

        if (counter == capacity) {
            var nodeToRemove = Head;
            Head = nodeToRemove.Next;
            Remove(nodeToRemove);
            Last.Next = Head;
            Head.Prev = Last;
        }
        
        map.Add(key, value);
        AddToLast(node);
        counter++;
    }

    private void Remove(LruNode node) 
    {
        node.Prev.next = node.Next;
        node.Next.Prev = node.Prev;
    }

    private void AddToLast(LruNode node) 
    {
        node.Prev = Last;
        node.Next = Head;

        Last.Next = node;
        Head.Prev = node;

        Last = node;
    }
}