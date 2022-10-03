using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

public interface IIndexed
{
    int IndexNo { set; get; }
}
public class ObservableSortedCollection<T> :
    ObservableCollection<T> where T : IComparable<T>, INotifyPropertyChanged, IIndexed
{

    #region Constructor

    // 1つ目はそのまま
    public ObservableSortedCollection() : base() { }

    // 2つ目はひとつずつ追加する
    public ObservableSortedCollection(IEnumerable<T> collection) : base()
    {
        foreach (var item in collection)
        {
            this.Add(item);
        }
    }

    public new void Add(T item)
    {
        AddItem(0, item);
    }
    #endregion

    #region Find
    
    public T Find(T target)
    {
        if (this.Count == 0) return default(T);
        var idx = findLast(target);
        if (idx == -1) return default(T);
        else if(idx > this.Count - 1)
        {
            return default(T);
        }
        else
        {
            if(this.ElementAt(idx).CompareTo(target) == 0)
            {
                return this.ElementAt(idx);
            }
            else
            {
                return default(T);
            }
        }
    }
    //targetを超えない最大の値を持つindexを返す。
    //存在しない場合（配列中の全ての値よりtargetが小さい場合
    //-1を返す。
    protected int findLast(T target)
    {
        if (base.Count == 0) return -1;
        int start = 0;
        int end = this.Count - 1;
        return find_iter(start, end, target);
    }
    private int find_iter(int start, int end, T target)
    {
        int mid = start + (end - start+1) / 2;
        int c = this.ElementAt(mid).CompareTo(target);

        if (start == mid)
        {
            if (c == 0)
            {
                return mid;
            }
            else if (c == 1)
            {
                return mid + 1;
            }
            else
            {
                return mid;
            }
        }
        else {

            if (c == 0)
            {
                return mid;
            }
            else if (c == 1)
            {
                return find_iter(mid, end, target);
            }
            else
            {
                return find_iter(start, mid-1, target);
            }
        }

    }
    #endregion
    #region FindLast

    public T FindLast(T target)
    {
        if (this.Count == 0) return default(T);
        var idx = findLast(target);
        if (idx < 0) return default(T);
        else if(idx > this.Count - 1)
        {
            return this.ElementAt(this.Count - 1);
        }
        else
        {
            return this.ElementAt(idx);
        }
    }

    #endregion


    #region InsertItem, MoveItem

    // 挿入
    protected int AddItem(int _, T item)
    {
        if(this.Count == 0)
        {
            base.InsertItem(0, item);
            item.PropertyChanged += OnPropertyChanged;
            item.IndexNo = 0;
            return 0;
        }
        var idx = findLast(item);
        if (idx == -1)
        {
            idx = 0;
        }
        
        base.InsertItem(idx, item);
        item.IndexNo = idx;
        for(int i = idx + 1; i < this.Count; i++)
        {
            this[i].IndexNo++;
        }
        item.PropertyChanged += OnPropertyChanged;
        return idx;
    }

    // 移動
    protected int SettleItem(int oldIndex, int _)
    {
        if (oldIndex < 0 || oldIndex > this.Count - 1) return -1;
        var temp = base[oldIndex];
        base.RemoveItem(oldIndex);
        return AddItem(0, temp);
    }

    #endregion
    #region Remove

    public new T Remove(T target)
    {
        int idx = findLast(target);
        var found = this.ElementAt(idx);
        if(found != null && found.CompareTo(target) == 0)
        {
            RemoveItem(idx);
            
            return found;
        }
        else
        {
            return default(T);
        }
    }

    #endregion

    #region RemoveItem, ClearItems

    // 削除
    protected override void RemoveItem(int index)
    {
        this[index].PropertyChanged -= OnPropertyChanged; // イベント変更通知を解除
        base.RemoveItem(index);
        for (int i = index; i < this.Count; i++)
        {
            this[i].IndexNo--;
        }
    }

    // クリア
    protected override void ClearItems()
    {
        foreach (var item in this)
        {
            item.PropertyChanged -= OnPropertyChanged; // イベント変更通知を解除
        }
        base.ClearItems();
    }

    #endregion

    #region OnPropertyChanged

    // Tのプロパティ変更時に呼び出される
    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
    }
    #endregion

}