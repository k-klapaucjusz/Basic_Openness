using System;
using System.Collections.Generic;
using static Basic_Openness.SclWrapper;
using Xunit;

namespace Basic_Openness
{
    internal class Tests
    {

    }


    public class OperandTests
    {
        [Fact]
        public void Constructor_ShouldSetMemoryAreaToLocalVariable_WhenDefaultConstructorIsUsed()
        {
            var operand = new Operand();
            Assert.Equal(MemoryAreas.LocalVariable, operand.MemoryArea);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenMemoryAreaIsLiteralConstant()
        {
            Assert.Throws<ArgumentException>(() => new Operand(MemoryAreas.LiteralConstant, "int", "TestName"));
        }

        [Fact]
        public void Constructor_ShouldSetProperties_WhenValidArgumentsArePassed()
        {
            var operand = new Operand(MemoryAreas.GlobalVariable, "int", "TestName");
            Assert.Equal(MemoryAreas.GlobalVariable, operand.MemoryArea);
            Assert.Equal("int", operand.DataType);
            Assert.Equal("TestName", operand.Name);
            Assert.Null(operand.InterfaceSection);
        }



        public class OperandPropertiesTests
        {
            [Fact]
            public void DataType_ShouldThrowArgumentException_WhenSetToNullOrEmpty()
            {
                var operand = new Operand();
                Assert.Throws<ArgumentException>(() => operand.DataType = null);
                Assert.Throws<ArgumentException>(() => operand.DataType = "");
            }

            [Fact]
            public void Name_ShouldReplaceWhitespacesWithUnderscores()
            {
                var operand = new Operand();
                operand.Name = "Test Name";
                Assert.Equal("Test_Name", operand.Name);
            }

            [Fact]
            public void Name_ShouldThrowArgumentException_WhenSetToNullOrEmpty()
            {
                var operand = new Operand();
                Assert.Throws<ArgumentException>(() => operand.Name = null);
                Assert.Throws<ArgumentException>(() => operand.Name = "");
            }

            [Fact]
            public void MemoryArea_ShouldThrowInvalidOperationException_WhenTryingToSetAfterInitialization()
            {
                var operand = new Operand();
                Assert.Throws<InvalidOperationException>(() => operand.MemoryArea = MemoryAreas.GlobalVariable);
            }

            [Fact]
            public void InterfaceSection_ShouldThrowInvalidOperationException_WhenMemoryAreaIsGlobalVariable()
            {
                var operand = new Operand(MemoryAreas.GlobalVariable, "int", "TestName");
                Assert.Throws<InvalidOperationException>(() => operand.InterfaceSection = InterfaceSections.Input);
            }

            [Fact]
            public void InterfaceSection_ShouldSet_WhenMemoryAreaIsLocalVariable()
            {
                var operand = new Operand();
                operand.InterfaceSection = InterfaceSections.Input;
                Assert.Equal(InterfaceSections.Input, operand.InterfaceSection);
            }

            [Fact]
            public void StartValue_ShouldThrowArgumentNullException_WhenDataTypeIsNull()
            {
                var operand = new Operand();
                Assert.Throws<ArgumentNullException>(() => operand.ValidateStartValue("SomeValue"));
            }

            [Fact]
            public void ValidateStartValue_ShouldThrowArgumentException_WhenStartValueIsNotAllowed()
            {
                var operand = new Operand();
                operand.DataType = "int";
                operand.InterfaceSection = InterfaceSections.Input;
                Assert.Throws<ArgumentException>(() => operand.ValidateStartValue("SomeInvalidValue"));
            }
        }

        public class OperandValidationTests
        {
            [Fact]
            public void ValidateProperties_ShouldSetAttributesAndIsRetainAndIsSetPointToNull_WhenInterfaceSectionIsConstant()
            {
                var operand = new Operand { InterfaceSection = InterfaceSections.Constant };
                Assert.Null(operand.Attributes);
                Assert.Null(operand.IsRetain);
                Assert.Null(operand.IsSetPoint);
            }

            [Fact]
            public void ValidateProperties_ShouldSetAttributesStartValueIsRetainAndIsSetPointToNull_WhenInterfaceSectionIsTemp()
            {
                var operand = new Operand { InterfaceSection = InterfaceSections.Temp };
                Assert.Null(operand.Attributes);
                Assert.Null(operand.StartValue);
                Assert.Null(operand.IsRetain);
                Assert.Null(operand.IsSetPoint);
            }

            [Fact]
            public void ValidateProperties_ShouldSetIsSetPointToNull_WhenInterfaceSectionIsInputOutputOrInOut()
            {
                var operand = new Operand { InterfaceSection = InterfaceSections.Input };
                Assert.Null(operand.IsSetPoint);

                operand.InterfaceSection = InterfaceSections.Output;
                Assert.Null(operand.IsSetPoint);

                operand.InterfaceSection = InterfaceSections.InOut;
                Assert.Null(operand.IsSetPoint);
            }

            [Fact]
            public void ValidateStartValue_ShouldReturnTrue_WhenStartValueIsConvertibleToDataType()
            {
                var operand = new Operand { DataType = "int", InterfaceSection = InterfaceSections.Static };
                Assert.True(operand.ValidateStartValue("123"));
            }
        }


    }


}
