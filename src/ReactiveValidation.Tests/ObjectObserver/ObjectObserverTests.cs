using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using FluentAssertions;
using Moq;
using Xunit;
using ReactiveValidation.ObjectObserver;
using ReactiveValidation.Tests.TestModels;

namespace ReactiveValidation.Tests.ObjectObserver
{
    /// <summary>
    /// Tests of <see cref="ObjectObserver{TObject}" /> class.
    /// </summary>
    public class ObjectObserverTests
    {
        #region Instance.PropertyChanged
        
        /// <summary>
        /// Check that <see cref="ObjectObserver{TObject}.PropertyChanged" /> raised if changed property of instance.
        /// </summary>
        [Fact]
        public void PropertyChanged_PropertyChanged_EventRaised()
        {
            // ARRANGE.
            var instance = new TestValidatableObject();
            var properties = new Dictionary<string, ObservingPropertySettings>
            {
                { nameof(instance.Number), new ObservingPropertySettings() }
            };
            var events = new List<PropertyChangedEventArgs>();
            var objectObserver = new ObjectObserver<TestValidatableObject>(instance, properties);
            objectObserver.PropertyChanged += (sender, args) => { events.Add(args); };
            
            // ACT.
            instance.Number = 1;

            // ASSERT.
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.Number));
        }
        
        /// <summary>
        /// Check that <see cref="ObjectObserver{TObject}.PropertyChanged" /> raised if changed property of instance.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData(nameof(TestValidatableObject.Number))]
        public void PropertyChanged_InstanceEventRaised_EventRaised(string? propertyName)
        {
            // ARRANGE.
            var instance = new TestValidatableObject();
            var properties = new Dictionary<string, ObservingPropertySettings>
            {
                { nameof(instance.Number), new ObservingPropertySettings() }
            };
            var events = new List<PropertyChangedEventArgs>();
            var objectObserver = new ObjectObserver<TestValidatableObject>(instance, properties);
            objectObserver.PropertyChanged += (sender, args) => { events.Add(args); };
            
            // ACT.
            instance.RaisePropertyChangedEvent(propertyName);

            // ASSERT.
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(propertyName);
        }

        #endregion

        #region Settings
        
        /// <summary>
        /// Check <see cref="ObservingPropertySettings.TrackValueChanged" /> setting.
        /// <see cref="ObjectObserver{TObject}.PropertyChanged" /> has to be raised if changed property of property of instance. 
        /// </summary>
        [Fact]
        public void Settings_ValueChanged_EventRaised()
        {
            // ARRANGE.
            var instance = new TestValidatableObject();
            var properties = new Dictionary<string, ObservingPropertySettings>
            {
                { nameof(instance.InnerValidatableObject), new ObservingPropertySettings() { TrackValueChanged = true} }
            };
            var events = new List<PropertyChangedEventArgs>();
            var objectObserver = new ObjectObserver<TestValidatableObject>(instance, properties);
            objectObserver.PropertyChanged += (sender, args) => { events.Add(args); };
            
            // ACT + ASSERT.
            // Step 1 -  create and set inner object.
            var firstValidatableObject = new TestValidatableObject();
            instance.InnerValidatableObject = firstValidatableObject;
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.InnerValidatableObject));
            events.Clear();

            // Step 2 - change property of inner object.
            firstValidatableObject.Number = 1;
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.InnerValidatableObject));
            events.Clear();
            
            // Step 3 - change inner object.
            var secondValidatableObject = new TestValidatableObject();
            instance.InnerValidatableObject = secondValidatableObject;
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.InnerValidatableObject));
            events.Clear();
            
            // Step 4 - check unsubscription of first object.
            firstValidatableObject.Number = 10;
            events.Count.Should().Be(0);
            
            // Step 5 - change inner property to null.
            instance.InnerValidatableObject = null;
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.InnerValidatableObject));
            events.Clear();
            
            // Step 6 - check unsubscription of second object.
            secondValidatableObject.Number = 10;
            events.Count.Should().Be(0);
        }
        
        /// <summary>
        /// Check <see cref="ObservingPropertySettings.TrackValueErrorsChanged" /> setting.
        /// <see cref="ObjectObserver{TObject}.PropertyChanged" /> has to be raised if errors of property changed. 
        /// </summary>
        [Fact]
        public void Settings_ValueErrorsChanged_EventRaised()
        {
            // ARRANGE.
            var instance = new TestValidatableObject();
            var properties = new Dictionary<string, ObservingPropertySettings>
            {
                { nameof(instance.InnerValidatableObject), new ObservingPropertySettings() { TrackValueErrorsChanged = true} }
            };
            var events = new List<PropertyChangedEventArgs>();
            var objectObserver = new ObjectObserver<TestValidatableObject>(instance, properties);
            objectObserver.PropertyChanged += (sender, args) => { events.Add(args); };
            
            // ACT + ASSERT.
            // Step 1 -  create and set inner object.
            var firstValidatableObject = new TestValidatableObject();
            instance.InnerValidatableObject = firstValidatableObject;
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.InnerValidatableObject));
            events.Clear();

            // Step 2 - raise messages changed of inner object.
            firstValidatableObject.OnPropertyMessagesChanged(nameof(TestValidatableObject.Number));
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.InnerValidatableObject));
            events.Clear();
            
            // Step 3 - change inner object.
            var secondValidatableObject = new TestValidatableObject();
            instance.InnerValidatableObject = secondValidatableObject;
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.InnerValidatableObject));
            events.Clear();
            
            // Step 4 - check unsubscription of first object.
            firstValidatableObject.OnPropertyMessagesChanged(nameof(TestValidatableObject.Number));
            events.Count.Should().Be(0);
            
            // Step 5 - change inner property to null.
            instance.InnerValidatableObject = null;
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.InnerValidatableObject));
            events.Clear();
            
            // Step 6 - check unsubscription of second object.
            secondValidatableObject.OnPropertyMessagesChanged(nameof(TestValidatableObject.Number));
            events.Count.Should().Be(0);
        }
        
        /// <summary>
        /// Check <see cref="ObservingPropertySettings.PropertyValueFactoryMethod" /> setting.
        /// For <see cref="IValidatableObject" /> property of instance has to be created and removed <see cref="IValidatableObject.Validator" />.
        /// </summary>
        [Fact]
        public void Settings_PropertyValueFactoryMethod_ValidatorSetAndRemoved()
        {
            // ARRANGE.
            var instance = new TestValidatableObject();
            var properties = new Dictionary<string, ObservingPropertySettings>
            {
                { nameof(instance.InnerValidatableObject), new ObservingPropertySettings { PropertyValueFactoryMethod = o => Mock.Of<IObjectValidator>() } }
            };
            var events = new List<PropertyChangedEventArgs>();
            var objectObserver = new ObjectObserver<TestValidatableObject>(instance, properties);
            objectObserver.PropertyChanged += (sender, args) => { events.Add(args); };
            
            // ACT + ASSERT.
            // Step 1 -  create and set inner object.
            var firstValidatableObject = new TestValidatableObject();
            instance.InnerValidatableObject = firstValidatableObject;
            
            var firstObjectValidator = Mock.Get(firstValidatableObject.Validator);
            firstObjectValidator.Should().NotBeNull();
            firstObjectValidator.Verify(v => v.Revalidate(), Times.Once);
            firstObjectValidator.VerifyNoOtherCalls();
            
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.InnerValidatableObject));
            events.Clear();
            
            // Step 2 - change inner object.
            var secondValidatableObject = new TestValidatableObject();
            instance.InnerValidatableObject = secondValidatableObject;

            firstValidatableObject.Validator.Should().BeNull();
            firstObjectValidator.Verify(v => v.Dispose(), Times.Once);
            firstObjectValidator.VerifyNoOtherCalls();

            var secondObjectValidator = Mock.Get(secondValidatableObject.Validator);
            secondObjectValidator.Should().NotBeNull();
            secondObjectValidator.Verify(v => v.Revalidate(), Times.Once);
            secondObjectValidator.VerifyNoOtherCalls();
            
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.InnerValidatableObject));
            events.Clear();
        }
        
        /// <summary>
        /// Check <see cref="ObservingPropertySettings.TrackCollectionChanged" /> setting.
        /// <see cref="ObjectObserver{TObject}.PropertyChanged" /> has to be raised if property-collection of instance is changed. 
        /// </summary>
        [Fact]
        public void Settings_CollectionChanged_EventRaised()
        {
            // ARRANGE.
            var instance = new TestValidatableObject();
            var properties = new Dictionary<string, ObservingPropertySettings>
            {
                { nameof(instance.Collection), new ObservingPropertySettings() { TrackCollectionChanged = true} }
            };
            var events = new List<PropertyChangedEventArgs>();
            var objectObserver = new ObjectObserver<TestValidatableObject>(instance, properties);
            objectObserver.PropertyChanged += (sender, args) => { events.Add(args); };
            
            // ACT + ASSERT.
            // Step 1 -  create and set collection.
            var firstCollection = new ObservableCollection<TestValidatableObject?>();
            instance.Collection = firstCollection;
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.Collection));
            events.Clear();

            // Step 2 - add new object to collection.
            firstCollection.Add(new TestValidatableObject());
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.Collection));
            events.Clear();
            
            // Step 3 - change collection.
            var secondCollection = new ObservableCollection<TestValidatableObject?>();
            instance.Collection = secondCollection;
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.Collection));
            events.Clear();
            
            // Step 4 - check unsubscription of first object.
            firstCollection.Add(new TestValidatableObject());
            events.Count.Should().Be(0);
            
            // Step 5 - change inner property to null.
            instance.Collection = null;
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.Collection));
            events.Clear();
            
            // Step 6 - check unsubscription of second object.
            secondCollection.Add(new TestValidatableObject());
            events.Count.Should().Be(0);
        }
        
        /// <summary>
        /// Check <see cref="ObservingPropertySettings.TrackCollectionItemChanged" /> setting.
        /// <see cref="ObjectObserver{TObject}.PropertyChanged" /> has to be raised if one of item of property-collection of instance is changed. 
        /// </summary>
        [Fact]
        public void Settings_CollectionItemChanged_EventRaised()
        {
            // ARRANGE.
            var collection = new ObservableCollection<TestValidatableObject?>();
            var instance = new TestValidatableObject { Collection = collection };
            var properties = new Dictionary<string, ObservingPropertySettings>
            {
                { nameof(instance.Collection), new ObservingPropertySettings() { TrackCollectionChanged = true, TrackCollectionItemChanged = true} }
            };
            var events = new List<PropertyChangedEventArgs>();
            var objectObserver = new ObjectObserver<TestValidatableObject>(instance, properties);
            objectObserver.PropertyChanged += (sender, args) => { events.Add(args); };
            
            // ACT + ASSERT.
            // Step 1 -  create and add item to collection.
            var firstObject = new TestValidatableObject();
            collection.Add(firstObject);
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.Collection));
            events.Clear();
            
            // Step 2 - change item.
            firstObject.Number = 1;
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.Collection));
            events.Clear();
            
            // Step 3 - remove item.
            collection.Remove(firstObject);
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.Collection));
            events.Clear();
            
            // Step 4 - change removed item.
            firstObject.Number = 10;
            events.Count.Should().Be(0);
            
            // Step 5 - add and remove null item.
            collection.Add(null);
            collection.Remove(null);
            events.Count.Should().Be(2);
            events[0].PropertyName.Should().Be(nameof(instance.Collection));
            events[1].PropertyName.Should().Be(nameof(instance.Collection));
            events.Clear();
            
            // Step 6 - add item and change collection.
            var secondObject = new TestValidatableObject();
            collection.Add(secondObject);
            instance.Collection = null;
            events.Count.Should().Be(2);
            events[0].PropertyName.Should().Be(nameof(instance.Collection));
            events[1].PropertyName.Should().Be(nameof(instance.Collection));
            events.Clear();
            
            // Step 7 - change object from removed collection.
            secondObject.Number = 10;
            events.Count.Should().Be(0);
        }
        
        /// <summary>
        /// Check <see cref="ObservingPropertySettings.TrackCollectionItemErrorsChanged" /> setting.
        /// <see cref="ObjectObserver{TObject}.PropertyChanged" /> has to be raised if one of item of property-collection of instance is changed his errors. 
        /// </summary>
        [Fact]
        public void Settings_CollectionItemErrorsChanged_EventRaised()
        {
            // ARRANGE.
            var collection = new ObservableCollection<TestValidatableObject?>();
            var instance = new TestValidatableObject { Collection = collection };
            var properties = new Dictionary<string, ObservingPropertySettings>
            {
                { nameof(instance.Collection), new ObservingPropertySettings() { TrackCollectionChanged = true, TrackCollectionItemErrorsChanged = true} }
            };
            var events = new List<PropertyChangedEventArgs>();
            var objectObserver = new ObjectObserver<TestValidatableObject>(instance, properties);
            objectObserver.PropertyChanged += (sender, args) => { events.Add(args); };
            
            // ACT + ASSERT.
            // Step 1 -  create and add item to collection.
            var firstObject = new TestValidatableObject();
            collection.Add(firstObject);
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.Collection));
            events.Clear();
            
            // Step 2 - raise messages changed of item.
            firstObject.OnPropertyMessagesChanged(nameof(TestValidatableObject.Number));
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.Collection));
            events.Clear();
            
            // Step 3 - remove item.
            collection.Remove(firstObject);
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.Collection));
            events.Clear();
            
            // Step 4 - change removed item.
            firstObject.OnPropertyMessagesChanged(nameof(TestValidatableObject.Number));
            events.Count.Should().Be(0);
            
            // Step 5 - add and remove null item.
            collection.Add(null);
            collection.Remove(null);
            events.Count.Should().Be(2);
            events[0].PropertyName.Should().Be(nameof(instance.Collection));
            events[1].PropertyName.Should().Be(nameof(instance.Collection));
            events.Clear();
            
            // Step 6 - add item and change collection.
            var secondObject = new TestValidatableObject();
            collection.Add(secondObject);
            instance.Collection = null;
            events.Count.Should().Be(2);
            events[0].PropertyName.Should().Be(nameof(instance.Collection));
            events[1].PropertyName.Should().Be(nameof(instance.Collection));
            events.Clear();
            
            // Step 7 - raise event for object from removed collection.
            secondObject.OnPropertyMessagesChanged(nameof(TestValidatableObject.Number));
            events.Count.Should().Be(0);
        }
        
        /// <summary>
        /// Check <see cref="ObservingPropertySettings.CollectionItemFactoryMethod" /> setting.
        /// For property-collection of <see cref="IValidatableObject" /> items has to be created and removed <see cref="IValidatableObject.Validator" />.
        /// </summary>
        [Fact]
        public void Settings_CollectionItemFactoryMethod_ValidatorSetAndRemoved()
        {
            // ARRANGE.
            var collection = new ObservableCollection<TestValidatableObject?>();
            var instance = new TestValidatableObject { Collection = collection };
            var properties = new Dictionary<string, ObservingPropertySettings>
            {
                { nameof(instance.Collection), new ObservingPropertySettings { TrackCollectionChanged = true, CollectionItemFactoryMethod = _ => new Mock<IObjectValidator>().Object} }
            };
            var events = new List<PropertyChangedEventArgs>();
            var objectObserver = new ObjectObserver<TestValidatableObject>(instance, properties);
            objectObserver.PropertyChanged += (sender, args) => { events.Add(args); };
            
            // ACT + ASSERT.
            // Step 1 -  create and add item to collection.
            var firstObject = new TestValidatableObject();
            collection.Add(firstObject);

            var firstObjectValidator = Mock.Get(firstObject.Validator);
            firstObjectValidator.Should().NotBeNull();
            firstObjectValidator.Verify(v => v.Revalidate(), Times.Once);
            firstObjectValidator.VerifyNoOtherCalls();
            
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.Collection));
            events.Clear();

            // Step 2 - remove item.
            collection.Remove(firstObject);

            firstObject.Validator.Should().BeNull();
            firstObjectValidator.Verify(v => v.Dispose(), Times.Once);
            firstObjectValidator.VerifyNoOtherCalls();
            
            events.Count.Should().Be(1);
            events[0].PropertyName.Should().Be(nameof(instance.Collection));
            events.Clear();

            // Step 3 - add and remove null item.
            collection.Add(null);
            collection.Remove(null);
            events.Count.Should().Be(2);
            events[0].PropertyName.Should().Be(nameof(instance.Collection));
            events[1].PropertyName.Should().Be(nameof(instance.Collection));
            events.Clear();
            
            // Step 4 - add item and change collection.
            var secondObject = new TestValidatableObject();
            collection.Add(secondObject);
            
            var secondObjectValidator = Mock.Get(secondObject.Validator);
            secondObjectValidator.Should().NotBeNull();
            secondObjectValidator.Verify(v => v.Revalidate(), Times.Once);
            secondObjectValidator.VerifyNoOtherCalls();
            
            instance.Collection = null;

            secondObject.Validator.Should().BeNull();
            secondObjectValidator.Verify(v => v.Dispose(), Times.Once);
            secondObjectValidator.VerifyNoOtherCalls();
            
            events.Count.Should().Be(2);
            events[0].PropertyName.Should().Be(nameof(instance.Collection));
            events[1].PropertyName.Should().Be(nameof(instance.Collection));
            events.Clear();
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Check that after <see cref="ObjectObserver{T}.Dispose" /> there is no raised events.
        /// </summary>
        [Fact]
        public void Dispose_InstancePropertyChanged_EventNotRaised()
        {
            // ARRANGE.
            var instance = new TestValidatableObject();
            var properties = new Dictionary<string, ObservingPropertySettings>
            {
                { nameof(instance.Number), new ObservingPropertySettings() }
            };
            var events = new List<PropertyChangedEventArgs>();
            var objectObserver = new ObjectObserver<TestValidatableObject>(instance, properties);
            objectObserver.PropertyChanged += (sender, args) => { events.Add(args); };
            
            // ACT.
            objectObserver.Dispose();
            instance.Number = 1;

            // ASSERT.
            events.Should().BeEmpty();
        }

        /// <summary>
        /// Check that after <see cref="ObjectObserver{T}.Dispose" /> there is no raised events.
        /// </summary>
        [Fact]
        public void Dispose_ValueChanged_EventNotRaised()
        {
            // ARRANGE.
            var instance = new TestValidatableObject { InnerValidatableObject = new TestValidatableObject() };
            var properties = new Dictionary<string, ObservingPropertySettings>
            {
                { nameof(instance.InnerValidatableObject), new ObservingPropertySettings() { TrackValueChanged = true} }
            };
            var events = new List<PropertyChangedEventArgs>();
            var objectObserver = new ObjectObserver<TestValidatableObject>(instance, properties);
            objectObserver.PropertyChanged += (sender, args) => { events.Add(args); };
            
            // ACT.
            objectObserver.Dispose();
            instance.InnerValidatableObject.Number = 1;

            // ASSERT.
            events.Should().BeEmpty();
        }
        
        /// <summary>
        /// Check that after <see cref="ObjectObserver{T}.Dispose" /> there is no raised events.
        /// </summary>
        [Fact]
        public void Dispose_ValueErrorsChanged_EventNotRaised()
        {
            // ARRANGE.
            var instance = new TestValidatableObject { InnerValidatableObject = new TestValidatableObject() };
            var properties = new Dictionary<string, ObservingPropertySettings>
            {
                { nameof(instance.InnerValidatableObject), new ObservingPropertySettings() { TrackValueErrorsChanged = true} }
            };
            var events = new List<PropertyChangedEventArgs>();
            var objectObserver = new ObjectObserver<TestValidatableObject>(instance, properties);
            objectObserver.PropertyChanged += (sender, args) => { events.Add(args); };
            
            // ACT.
            objectObserver.Dispose();
            instance.InnerValidatableObject.OnPropertyMessagesChanged(nameof(TestValidatableObject.Number));

            // ASSERT.
            events.Should().BeEmpty();
        }
        
        /// <summary>
        /// Check that after <see cref="ObjectObserver{T}.Dispose" /> <see cref="IValidatableObject.Validator" /> removed from property.
        /// </summary>
        [Fact]
        public void Dispose_PropertyValueFactoryMethod_ValidatorRemoved()
        {
            // ARRANGE.
            var validator = new Mock<IObjectValidator>();
            var instance = new TestValidatableObject { InnerValidatableObject = new TestValidatableObject { Validator = validator.Object } };
            validator.Reset();
            var properties = new Dictionary<string, ObservingPropertySettings>
            {
                { nameof(instance.InnerValidatableObject), new ObservingPropertySettings { PropertyValueFactoryMethod = o => Mock.Of<IObjectValidator>() } }
            };
            var objectObserver = new ObjectObserver<TestValidatableObject>(instance, properties);

            // ACT.
            objectObserver.Dispose();

            // ASSERT.
            instance.InnerValidatableObject.Validator.Should().BeNull();
            validator.Verify(v => v.Dispose());
            validator.VerifyNoOtherCalls();
        }
        
        /// <summary>
        /// Check that after <see cref="ObjectObserver{T}.Dispose" /> there is no raised events.
        /// </summary>
        [Fact]
        public void Dispose_CollectionChanged_EventNotRaised()
        {
            // ARRANGE.
            var collection = new ObservableCollection<TestValidatableObject?>();
            var instance = new TestValidatableObject { Collection = collection };
            var properties = new Dictionary<string, ObservingPropertySettings>
            {
                { nameof(instance.Collection), new ObservingPropertySettings() { TrackCollectionChanged = true} }
            };
            var events = new List<PropertyChangedEventArgs>();
            var objectObserver = new ObjectObserver<TestValidatableObject>(instance, properties);
            objectObserver.PropertyChanged += (sender, args) => { events.Add(args); };
            
            // ACT.
            objectObserver.Dispose();
            collection.Add(new TestValidatableObject());

            // ASSERT.
            events.Should().BeEmpty();
        }
        
        /// <summary>
        /// Check that after <see cref="ObjectObserver{T}.Dispose" /> there is no raised events.
        /// </summary>
        [Fact]
        public void Dispose_CollectionItemChanged_EventNotRaised()
        {
            // ARRANGE.
            var item = new TestValidatableObject();
            var collection = new ObservableCollection<TestValidatableObject?> { item };
            var instance = new TestValidatableObject { Collection = collection };
            var properties = new Dictionary<string, ObservingPropertySettings>
            {
                { nameof(instance.Collection), new ObservingPropertySettings() { TrackCollectionItemChanged = true } }
            };
            var events = new List<PropertyChangedEventArgs>();
            var objectObserver = new ObjectObserver<TestValidatableObject>(instance, properties);
            objectObserver.PropertyChanged += (sender, args) => { events.Add(args); };
            
            // ACT.
            objectObserver.Dispose();
            item.Number = 1;

            // ASSERT.
            events.Should().BeEmpty();
        }
        
        /// <summary>
        /// Check that after <see cref="ObjectObserver{T}.Dispose" /> there is no raised events.
        /// </summary>
        [Fact]
        public void Dispose_CollectionItemErrorsChanged_EventNotRaised()
        {
            // ARRANGE.
            var item = new TestValidatableObject();
            var collection = new ObservableCollection<TestValidatableObject?> { item };
            var instance = new TestValidatableObject { Collection = collection };
            var properties = new Dictionary<string, ObservingPropertySettings>
            {
                { nameof(instance.Collection), new ObservingPropertySettings() { TrackCollectionItemErrorsChanged = true } }
            };
            var events = new List<PropertyChangedEventArgs>();
            var objectObserver = new ObjectObserver<TestValidatableObject>(instance, properties);
            objectObserver.PropertyChanged += (sender, args) => { events.Add(args); };
            
            // ACT.
            objectObserver.Dispose();
            item.OnPropertyMessagesChanged(nameof(TestValidatableObject.Number));

            // ASSERT.
            events.Should().BeEmpty();
        }
        
        /// <summary>
        /// Check that after <see cref="ObjectObserver{T}.Dispose" /> there is no raised events.
        /// </summary>
        [Fact]
        public void Dispose_CollectionItemFactoryMethod_ValidatorRemoved()
        {
            // ARRANGE.
            var validator = new Mock<IObjectValidator>();
            var item = new TestValidatableObject { Validator = validator.Object };
            validator.Reset();
            var collection = new ObservableCollection<TestValidatableObject?> { item };
            var instance = new TestValidatableObject { Collection = collection };
            var properties = new Dictionary<string, ObservingPropertySettings>
            {
                { nameof(instance.Collection), new ObservingPropertySettings { TrackCollectionChanged = true, CollectionItemFactoryMethod = _ => new Mock<IObjectValidator>().Object} }
            };
            var objectObserver = new ObjectObserver<TestValidatableObject>(instance, properties);
            
            // ACT.
            objectObserver.Dispose();
            
            // ASSERT.
            item.Validator.Should().BeNull();
            validator.Verify(v => v.Dispose());
            validator.VerifyNoOtherCalls();
        }
        
        #endregion
    }
}