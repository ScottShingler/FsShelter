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

namespace StormThrift
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class HBMessageData : TBase
  {
    private string _path;
    private HBPulse _pulse;
    private bool _boolval;
    private HBRecords _records;
    private HBNodes _nodes;
    private byte[] _message_blob;

    public string Path
    {
      get
      {
        return _path;
      }
      set
      {
        __isset.path = true;
        this._path = value;
      }
    }

    public HBPulse Pulse
    {
      get
      {
        return _pulse;
      }
      set
      {
        __isset.pulse = true;
        this._pulse = value;
      }
    }

    public bool Boolval
    {
      get
      {
        return _boolval;
      }
      set
      {
        __isset.boolval = true;
        this._boolval = value;
      }
    }

    public HBRecords Records
    {
      get
      {
        return _records;
      }
      set
      {
        __isset.records = true;
        this._records = value;
      }
    }

    public HBNodes Nodes
    {
      get
      {
        return _nodes;
      }
      set
      {
        __isset.nodes = true;
        this._nodes = value;
      }
    }

    public byte[] Message_blob
    {
      get
      {
        return _message_blob;
      }
      set
      {
        __isset.message_blob = true;
        this._message_blob = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool path;
      public bool pulse;
      public bool boolval;
      public bool records;
      public bool nodes;
      public bool message_blob;
    }

    public HBMessageData() {
    }

    public void Read (TProtocol iprot)
    {
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
              Path = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.Struct) {
              Pulse = new HBPulse();
              Pulse.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.Bool) {
              Boolval = iprot.ReadBool();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.Struct) {
              Records = new HBRecords();
              Records.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.Struct) {
              Nodes = new HBNodes();
              Nodes.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.String) {
              Message_blob = iprot.ReadBinary();
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
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("HBMessageData");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Path != null && __isset.path) {
        field.Name = "path";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Path);
        oprot.WriteFieldEnd();
      }
      if (Pulse != null && __isset.pulse) {
        field.Name = "pulse";
        field.Type = TType.Struct;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        Pulse.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (__isset.boolval) {
        field.Name = "boolval";
        field.Type = TType.Bool;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(Boolval);
        oprot.WriteFieldEnd();
      }
      if (Records != null && __isset.records) {
        field.Name = "records";
        field.Type = TType.Struct;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        Records.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (Nodes != null && __isset.nodes) {
        field.Name = "nodes";
        field.Type = TType.Struct;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        Nodes.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (Message_blob != null && __isset.message_blob) {
        field.Name = "message_blob";
        field.Type = TType.String;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        oprot.WriteBinary(Message_blob);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HBMessageData(");
      sb.Append("Path: ");
      sb.Append(Path);
      sb.Append(",Pulse: ");
      sb.Append(Pulse== null ? "<null>" : Pulse.ToString());
      sb.Append(",Boolval: ");
      sb.Append(Boolval);
      sb.Append(",Records: ");
      sb.Append(Records== null ? "<null>" : Records.ToString());
      sb.Append(",Nodes: ");
      sb.Append(Nodes== null ? "<null>" : Nodes.ToString());
      sb.Append(",Message_blob: ");
      sb.Append(Message_blob);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
