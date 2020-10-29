using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;


namespace Tree
{
    public partial class TreeTableView : UITableView, IUITableViewDelegate, IUITableViewDataSource
    {
        private List<NodeModel> _data;       //传递过来已经组织好的数据（全量数据）
        private List<NodeModel> _tempData;   //用于存储数据源（部分数据）

        public delegate void NodeCellDelegate(NodeModel node);
        public event NodeCellDelegate nodeCellDelegate;   //声明事件
      
        
        public TreeTableView(CGRect frame, List<NodeModel> data)
        {
            _data = data;
            _tempData = createTempData(data);
            BackgroundColor = UIColor.Blue;
            Delegate = (IUITableViewDelegate)this;
            DataSource = (IUITableViewDataSource)this;
            Frame = frame;
            ReloadData();
        }

        // 初始化数据源
        private List<NodeModel> createTempData(List<NodeModel> data)
        {
            List<NodeModel> tempArray = new List<NodeModel>();
            for (int i = 0; i < data.Count; i++)
            {
                NodeModel node = data[i];
                if (node._expand)
                {
                    tempArray.Add(node);
                }
            }
            return tempArray;
        }

        [Export("tableView:numberOfRowsInSection:")]
        nint IUITableViewDataSource.RowsInSection(UITableView tableView, nint section)
        {
            return _tempData.Count;
        }

        [Export("tableView:cellForRowAtIndexPath:")]
        UITableViewCell IUITableViewDataSource.GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = new UITableViewCell();
            cell.SelectionStyle = UITableViewCellSelectionStyle.Default;
            NodeModel node = _tempData[indexPath.Row];
            // cell有缩进的方法
            cell.IndentationLevel = node._depth;
            cell.IndentationWidth = 30;

            cell.TextLabel.Text = node._name;
            return cell;
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            // 先修改数据源
            NodeModel parentNode = _tempData[indexPath.Row];

            nodeCellDelegate?.Invoke(parentNode);

            int startPosition = indexPath.Row + 1;
            int endPosition = startPosition;
            bool expand = false;
            for (int i = 0; i < _data.Count; i++)
            {
                NodeModel node = _data[i];
                if (node._parentId == parentNode._nodeId)
                {
                    node._expand = !node._expand;
                    if (node._expand)
                    {
                        _tempData.Insert(endPosition, node);
                        expand = true;
                        endPosition++;
                    }
                    else
                    {
                        expand = false;
                        endPosition = removeAllNodesAtParentNode(parentNode);
                        break;
                    }
                }
            }
            ResetIndexPath(tableView, startPosition, endPosition, expand);
        }

        // 重置NSIndexPath
        private void ResetIndexPath(UITableView tableView, int startPosition, int endPosition, bool expand)
        {
            // 获得需要修正的indexPath
            List<NSIndexPath> indexPathArray = new List<NSIndexPath>();
            for (int i = startPosition; i < endPosition; i++)
            {
                NSIndexPath tempIndexPath = NSIndexPath.FromRowSection(i, 0);
                indexPathArray.Add(tempIndexPath);
            }

            // 插入或者删除相关节点
            if (expand)
            {
                tableView.InsertRows(indexPathArray.ToArray(), UITableViewRowAnimation.None);
            }
            else
            {
                tableView.DeleteRows(indexPathArray.ToArray(), UITableViewRowAnimation.None);
            }
        }

        // 删除该父节点下的所有子节点（包括孙子节点）
        private int removeAllNodesAtParentNode(NodeModel parentNode)
        {
            int startPosition = _tempData.IndexOf(parentNode);
            int endPosition = startPosition;
            for (int i = startPosition + 1; i < _tempData.Count; i++)
            {
                NodeModel node = _tempData[i];
                endPosition++;
                if (node._depth <= parentNode._depth)
                {
                    break;
                }
                if (endPosition == _tempData.Count - 1)
                {
                    endPosition++;
                    node._expand = false;
                    break;
                }
                node._expand = false;
            }
            if (endPosition > startPosition)
            {
                _tempData.RemoveRange(startPosition + 1, endPosition - startPosition - 1);
            }
            return endPosition;
        }
    }


    

}


/*
 NodeModel top_a = new NodeModel(-1,1, "亚洲国家", 0,true);
            NodeModel top_b = new NodeModel(-1, 2, "欧洲国家", 0, true);
            NodeModel top_c = new NodeModel(-1, 3, "美洲国家", 0, true);
            NodeModel top_d = new NodeModel(-1, 4, "非洲国家", 0, true);
            NodeModel top_e = new NodeModel(-1, 5, "大洋洲国家", 0, true);

            NodeModel GUO_a = new NodeModel(1, 10, "中国", 1, false);
            NodeModel GUO_c = new NodeModel(1, 20, "日本", 1, false);
            NodeModel GUO_d = new NodeModel(1, 30, "韩国", 1, false);
            NodeModel GUO_b = new NodeModel(1, 40, "蒙古国", 1, false);

            NodeModel province_a = new NodeModel(10, 100, "北京", 2, false);
            NodeModel province_b = new NodeModel(10, 200, "上海", 2, false);
            NodeModel province_c = new NodeModel(10, 300, "广东省", 2, false);
            NodeModel province_d = new NodeModel(10, 400, "深圳", 2, false);

            NodeModel municipal_a = new NodeModel(100, 1000, "北京市", 3, false);
            NodeModel municipal_a1 = new NodeModel(300, 1001, "广州市", 3, false);
            NodeModel municipal_b = new NodeModel(300, 2000, "深圳市", 3, false);
            NodeModel municipal_c = new NodeModel(300, 3000, "珠海市", 3, false);
            NodeModel municipal_d = new NodeModel(300, 4000, "汕头市", 3, false);

            NodeModel area_a = new NodeModel(1001, 10000, "越秀区", 4, false);
            NodeModel area_b = new NodeModel(1001, 20000, "荔湾区", 4, false);
            NodeModel area_c = new NodeModel(1001, 30000, "海珠区", 4, false);
            
            List<NodeModel> data = new List<NodeModel>();
            data.Add(top_a);
            data.Add(top_b);
            data.Add(top_c);
            data.Add(top_d);
            data.Add(top_e);

            data.Add(GUO_a);
            data.Add(GUO_b);
            data.Add(GUO_c);
            data.Add(GUO_d);

            data.Add(province_a);
            data.Add(province_b);
            data.Add(province_c);
            data.Add(province_d);

            data.Add(municipal_a1);
            data.Add(municipal_a);
            data.Add(municipal_b);
            data.Add(municipal_c);
            data.Add(municipal_d);

            data.Add(area_a);
            data.Add(area_b);
            data.Add(area_c);
        

            _treeTableView = new TreeTableView(View.Bounds, data);
            _treeTableView.nodeCellDelegate += tableViewCellClick;


            View.Add(_treeTableView);


/// other

// private TreeTableView _treeTableView;
        public void tableViewCellClick(NodeModel node)
        {
            System.Console.WriteLine("hello:" + node._name);
        }

 */