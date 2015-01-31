using System;
using System.Collections.Generic;
using System.Text;

namespace Moonlit.Collections
{
    /// <summary>
    /// 树型对象，专门处理将拥有类似于parent属性的对象打包出带有child属性的对象
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public class Tree<E>
    {
        #region Entity
        private E _entity;
        /// <summary>
        /// 元素对象
        /// </summary>
        public E Entity
        {
            get { return _entity; }
        }
        #endregion

        #region Childs
        private List<Tree<E>> _childs = new List<Tree<E>>();

        /// <summary>
        /// Gets or sets the childs.
        /// </summary>
        /// <value>The childs.</value>
        public List<Tree<E>> Childs
        {
            get { return _childs; }
            set { _childs = value; }
        }

        #endregion

        #region Ctor
        /// <summary>
        /// 构造一个Tree对象
        /// </summary>
        public Tree(E e)
        {
            this._entity = e;
        }
        #endregion

        #region delegates
        /// <summary>
        /// 是否可以添加到子节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public delegate bool EnabledAddEventHandler(E parent, E child);
        /// <summary>
        /// 判断是否根节点
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public delegate bool IsRootEventHandler(E entity);
        #endregion

        #region BuildTrees
        /// <summary>
        /// 构建树，树中只包含第一级的对象
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="enableAdd"></param>
        /// <param name="isRoot"></param>
        /// <returns></returns>
        public static List<Tree<E>> BuildTrees(ICollection<E> entities, IsRootEventHandler isRoot, EnabledAddEventHandler enableAdd)
        {
            List<Tree<E>> trees = new List<Tree<E>>();

            foreach (E entity in entities)
            {
                if (isRoot(entity))
                {
                    Tree<E> tree = new Tree<E>(entity);
                    trees.Add(tree);

                    BuildChildrens(entities, enableAdd, tree);
                }
            }

            return trees;
        }

        private static void BuildChildrens(ICollection<E> entities, EnabledAddEventHandler enableAdd, Tree<E> tree)
        {
            foreach (E e in entities)
            {
                if (enableAdd(tree.Entity, e))
                {
                    Tree<E> childTree = new Tree<E>(e);
                    tree.Childs.Add(childTree);
                    BuildChildrens(entities, enableAdd, childTree);
                }
            }
        }
        #endregion
    }
}
