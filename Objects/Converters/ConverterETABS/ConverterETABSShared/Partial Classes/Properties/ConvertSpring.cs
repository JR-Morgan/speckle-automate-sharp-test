﻿using System;
using System.Collections.Generic;
using Objects.Structural.Properties.Profiles;
using ETABSv1;
using System.Linq;
using Objects.Structural.ETABS.Properties;

namespace Objects.Converter.ETABS
{
  public partial class ConverterETABS
  {
    public void SpringPropertyToNative(ETABSSpringProperty springProperty){
      double[] k = new double[6];
      k[0] = springProperty.stiffnessX;
      k[1] = springProperty.stiffnessY;
      k[2] = springProperty.stiffnessZ;
      k[3] = springProperty.stiffnessXX;
      k[4] = springProperty.stiffnessYY;
      k[5] = springProperty.stiffnessZZ;
      switch(springProperty.springOption){
        case SpringOption.Link:
          var springOption = 1;
          Model.PropPointSpring.SetPointSpringProp(springProperty.name, springOption, ref k, springProperty.CYs, iGUID: springProperty.applicationId);
          break;
        case SpringOption.SoilProfileFooting:
          springOption = 2;
          throw new NotSupportedException();
          break;
      }

    }
    public void LinearSpringPropertyToNative(ETABSLinearSpring linearSpringProperty){

    }
    public void AreaSpringPropertyToNative(ETABSAreaSpring areaSpring){

    }
    public ETABSSpringProperty SpringPropertyToSpeckle(string name)
    {
      double[] stiffness = null;
      int springOption = 0;
      string Cys = null;
      string soilProfile = null;
      string footing = null;
      double period = 0;
      int color = 0;
      string notes = null;
      string GUID = null;
      Model.PropPointSpring.GetPointSpringProp(name, ref springOption, ref stiffness, ref Cys, ref soilProfile, ref footing, ref period, ref color, ref notes, ref GUID);
      switch (springOption)
      {
        case 1:
          ETABSSpringProperty speckleSpringProperty = new ETABSSpringProperty(name, Cys, stiffness[0], stiffness[1], stiffness[2], stiffness[3], stiffness[4], stiffness[5]);
          speckleSpringProperty.applicationId = GUID;
          return speckleSpringProperty;
        case 2:
          speckleSpringProperty = new ETABSSpringProperty(name, soilProfile, footing, period);
          speckleSpringProperty.applicationId = GUID;
          return speckleSpringProperty;
        default:
          speckleSpringProperty = new ETABSSpringProperty();
          return speckleSpringProperty;
      }
    }
    public ETABSLinearSpring LinearSpringToSpeckle(string name)
    {
      double stiffnessX = 0;
      double stiffnessY = 0;
      double stiffnessZ = 0;
      double stiffnessXX = 0;
      int nonLinearOpt1 = 0;
      int nonLinearOpt2 = 0;
      int color = 0;
      string notes = null;
      string GUID = null;
      NonLinearOptions nonLinearOptions1 = NonLinearOptions.Linear;
      NonLinearOptions nonLinearOptions2 = NonLinearOptions.Linear;

      var s = Model.PropLineSpring.GetLineSpringProp(name, ref stiffnessX, ref stiffnessY, ref stiffnessZ, ref stiffnessXX, ref nonLinearOpt1, ref nonLinearOpt2, ref color, ref notes, ref GUID);
      switch(nonLinearOpt1){
        case 0:
          nonLinearOptions1 = NonLinearOptions.Linear;
          break;
        case 1:
          nonLinearOptions1 = NonLinearOptions.CompressionOnly;
          break;
        case 2:
          nonLinearOptions1 = NonLinearOptions.TensionOnly;
          break;
      }
      switch(nonLinearOpt2){
        case 0:
          nonLinearOptions2 = NonLinearOptions.Linear;
          break;
        case 1:
          nonLinearOptions2 = NonLinearOptions.CompressionOnly;
          break;
        case 2:
          nonLinearOptions2 = NonLinearOptions.TensionOnly;
          break;
      }

      if(s == 0)
      {
        ETABSLinearSpring speckleLinearSpring = new ETABSLinearSpring(name, stiffnessX, stiffnessY, stiffnessZ, stiffnessXX, nonLinearOptions1, nonLinearOptions2, GUID);
        return speckleLinearSpring;
        }
      return null;

    }
    public ETABSAreaSpring AreaSpringToSpeckle(string name)
    {

      double stiffnessX = 0;
      double stiffnessY = 0;
      double stiffnessZ = 0;

      int nonLinearOpt1 = 0;
      int springOption = 0;
      string soilProfile = null;
      double endLengthRatio = 0;
      double period = 0;

      int color = 0;
      string notes = null;
      string GUID = null;
      NonLinearOptions nonLinearOptions1 = NonLinearOptions.Linear;

      var s = Model.PropAreaSpring.GetAreaSpringProp(name, ref stiffnessX, ref stiffnessY, ref stiffnessZ, ref nonLinearOpt1,ref springOption,ref soilProfile, ref endLengthRatio, ref period, ref color, ref notes, ref GUID);
      switch (nonLinearOpt1)
      {
        case 0:
          nonLinearOptions1 = NonLinearOptions.Linear;
          break;
        case 1:
          nonLinearOptions1 = NonLinearOptions.CompressionOnly;
          break;
        case 2:
          nonLinearOptions1 = NonLinearOptions.TensionOnly;
          break;
      }

      if (s == 0)
      {
        ETABSAreaSpring speckleAreaSpring = new ETABSAreaSpring(name, stiffnessX, stiffnessY, stiffnessZ,  nonLinearOptions1, GUID);
        return speckleAreaSpring;
      }
      return null;

    }
  }
}
