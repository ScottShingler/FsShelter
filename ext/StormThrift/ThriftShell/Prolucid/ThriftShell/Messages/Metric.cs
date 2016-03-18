/**
 * Autogenerated by Thrift Compiler (0.9.1)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace Prolucid.ThriftShell.Messages
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class Metric : TBase
  {

    public string Name { get; set; }

    public List<Variant> Params { get; set; }

    public Metric() {
    }

    public Metric(string name, List<Variant> @params) : this() {
      this.Name = name;
      this.Params = @params;
    }

    public void Read (TProtocol iprot)
    {
      bool isset_name = false;
      bool isset_params = false;
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.String) {
              Name = iprot.ReadString();
              isset_name = true;
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.List) {
              {
                Params = new List<Variant>();
                TList _list26 = iprot.ReadListBegin();
                for( int _i27 = 0; _i27 < _list26.Count; ++_i27)
                {
                  Variant _elem28 = new Variant();
                  _elem28 = new Variant();
                  _elem28.Read(iprot);
                  Params.Add(_elem28);
                }
                iprot.ReadListEnd();
              }
              isset_params = true;
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
      if (!isset_name)
        throw new TProtocolException(TProtocolException.INVALID_DATA);
      if (!isset_params)
        throw new TProtocolException(TProtocolException.INVALID_DATA);
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("Metric");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      field.Name = "name";
      field.Type = TType.String;
      field.ID = 1;
      oprot.WriteFieldBegin(field);
      oprot.WriteString(Name);
      oprot.WriteFieldEnd();
      field.Name = "params";
      field.Type = TType.List;
      field.ID = 2;
      oprot.WriteFieldBegin(field);
      {
        oprot.WriteListBegin(new TList(TType.Struct, Params.Count));
        foreach (Variant _iter29 in Params)
        {
          _iter29.Write(oprot);
        }
        oprot.WriteListEnd();
      }
      oprot.WriteFieldEnd();
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("Metric(");
      sb.Append("Name: ");
      sb.Append(Name);
      sb.Append(",Params: ");
      sb.Append(Params);
      sb.Append(")");
      return sb.ToString();
    }

  }

}