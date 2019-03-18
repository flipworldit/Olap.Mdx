using System;
using System.Collections.Generic;
using System.Linq;
using LinqExtIn;

namespace OLAP.Mdx.MdxElements
{
    public class MdxHierarchy
        : IMdxElement
    {
        public IMdxCollectionElements parentMdx;

        public string Name;
        private bool KeyProperties = false;
        public bool _children = false;
        public bool _currentMember = false;
        public bool _prevMember = false;
        public bool _members = false;
        public int _parent = 0;



        public MdxHierarchy(string name)
        {
            Name = name;

            if (string.IsNullOrEmpty(Name))
            {
                throw new ArgumentException("Имя измерения не может быть пустым", "name");
            }
        }

        public MdxHierarchy(MdxHierarchy toCopy)
            :this(toCopy.Name)
        {
            KeyProperties = toCopy.KeyProperties;

            _children = toCopy._children;
            _currentMember = toCopy._currentMember;
            _prevMember = toCopy._prevMember;
            _members = toCopy._members;
            _parent = toCopy._parent;
        }

        public virtual MdxValueElement Lag(object lagValue)
        {
            return new MdxValueElement(this, null, null, lagValue);
        }

        public MdxHierarchy Children()
        {
            _children = true;

            return this;
        }

        public MdxHierarchy Parent()
        {
            _parent++;

            return this;
        }

        public MdxHierarchy CurrentMember()
        {
            _currentMember = true;

            return this;
        }
        public MdxHierarchy PrevMember()
        {
            _prevMember = true;

            return this;
        }

        public MdxHierarchy KeyPropertie()
        {
            KeyProperties = true;

            return this;
        }

        public MdxHierarchy Members()
        {
            _members = true;

            return this;
        }

        public virtual void Draw(MdxDrawContext dc)
        {
            dc.Append(Name);

            if (_children)
            {
                dc.Append(".children");
            }

            if (_currentMember)
            {
                dc.Append(".CurrentMember");
            }

            if (_prevMember)
            {
                dc.Append(".PrevMember");
            }

            if (KeyProperties)
            {
                dc.Append(".Properties(\"Key\")");
            }

            if (_members)
            {
                dc.Append(".Members");
            }

            for (var i = 0; i < _parent; i++)
            {
                dc.Append(".Parent");
            }
        }

        public IEnumerable<IMdxElement> GetChildren()
        {
            return new List<IMdxElement>();
        }

        public IMdxElement NotEmpty()
        {
            return new MdxNonEmpty(this);
        }

        public IMdxElement WithEmpty()
        {
            return new MdxWithEmpty(this);
        }

        public MdxValueElement Value(string value)
        {
            var valueBuilder = new MdxValueElement(this, value ?? "");

            return valueBuilder;
        }

        public MdxValueElement Value(string[] value)
        {
            var secondValue = value.Length > 1 ? value[1] : null;

            var valueBuilder = new MdxValueElement(this, value[0] ?? "", secondValue: secondValue);

            return valueBuilder;
        }

        public IMdxElement Range(string value1, string value2)
        {
            return new MdxRangeElement(Value(value1), Value(value2));
        }
    }
}