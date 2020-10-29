using System;

namespace Tree
{
    public class NodeModel
    { 
        public int _parentId;   //父节点的id，如果为-1表示该节点为根节点
        public int _nodeId;     //本节点的id
        public string _name;    //本节点的名称
        public int _depth;      //该节点的深度
        public bool _expand;    //该节点是否处于展开状态'


        public NodeModel(int parentId, int nodeId, string name, int depth, bool expand)
        {
            _parentId = parentId;
            _nodeId = nodeId;
            _name = name;
            _depth = depth;
            _expand = expand;
        }
    }

}
