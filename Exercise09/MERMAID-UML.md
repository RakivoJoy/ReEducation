# Garage 3.0 - UML class diagram

```mermaid
classDiagram
	direction TB

	class IVehicle {
		<<interface>>
		+string RegistrationNumber
		+string Color
		+int NumberOfWheels
		+VehicleType Type
		+int ParkingSize
	}

	class Vehicle {
		<<abstract>>
		+string RegistrationNumber
		+string Color
		+int NumberOfWheels
		+VehicleType Type
		+int ParkingSize
		#string Details
		+ToString() string
	}

	class Airplane {
		+int NumberOfEngines
	}
	class Boat {
		+double Length
	}
	class Bus {
		+int NumberOfSeats
	}
	class Car {
		+FuelType FuelType
	}
	class Motorcycle {
		+int CylinderVolume
	}
	class Truck {
		+int LoadCapacity
	}

	class VehicleType {
		<<enumeration>>
		Motorcycle
		Airplane
		Car
		Bus
		Boat
		Truck
	}

	class FuelType {
		<<enumeration>>
		Gasoline
		Diesel
	}

	class VehicleColors {
		<<static>>
		+Normalize(string color) string
	}

	class SampleData {
		<<static>>
		+CreateSampleVehicles() List~IVehicle~
	}

	class Garage~T~ {
		+int Capacity
		+int MaxCapacity$
		+Garage(int capacity)
		+AddVehicle(T vehicle) OperationResult
		+RemoveVehicleByRegistration(string registrationNumber) OperationResult
		+Populate(IEnumerable~T~ vehicles) OperationResult
		+ListAllVehicles() IReadOnlyList~T~
		+ListVehicleCountByType() Dictionary~VehicleType, int~
		+FindByRegistration(string registrationNumber) T
		+SearchByProperties(SearchCriteria criteria) IReadOnlyList~T~
	}

	class SearchCriteria {
		+IReadOnlyList~string~ PropertyNames$
		+bool TrySetFilter(string property, string text, out string error)
		+bool Matches(IVehicle vehicle)
	}

	class IntRange {
		<<record struct>>
		+int Min
		+int Max
		+bool Contains(int value)
		+TryParse(string text, out IntRange range)$ bool
	}

	class OperationResult {
		<<record>>
		+bool Success
		+string Message
		+Ok(string message)$ OperationResult
		+Fail(string message)$ OperationResult
	}

	class ParkingSizeLimits {
		<<static>>
		+For(VehicleType type)$ (int Min, int Max)
	}

	class IHandler {
		<<interface>>
		+bool HasGarage
		+CreateGarage(int capacity) OperationResult
		+PopulateGarage() OperationResult
		+ParkVehicle(IVehicle vehicle) OperationResult
		+RemoveVehicle(string registrationNumber) OperationResult
		+ListVehicles() IReadOnlyList~IVehicle~
		+ListVehicleCounts() Dictionary~VehicleType, int~
		+FindVehicle(string registrationNumber) IVehicle
		+SearchVehicles(SearchCriteria criteria) IReadOnlyList~IVehicle~
		+SaveGarage() OperationResult
		+LoadGarage() OperationResult
	}

	class GarageHandler {
		-Garage~IVehicle~ _garage
		-IGarageStorage _storage
	}

	class IGarageStorage {
		<<interface>>
		+SaveAsync(Garage~IVehicle~ garage) Task~OperationResult~
		+LoadAsync() Task~GarageLoadResult~
	}

	class JsonGarageStorage {
		-string _filePath
		+string DefaultFilePath$
		+JsonGarageStorage(string filePath)
	}

	class GarageLoadResult {
		<<record>>
		+bool Success
		+string Message
		+Garage~IVehicle~ Garage
		+Ok(Garage~IVehicle~ garage, string message)$ GarageLoadResult
		+Fail(string message)$ GarageLoadResult
	}

	class GarageMapper {
		<<static>>
		+int CurrentVersion$
		+ToDto(Garage~IVehicle~ garage)$ GarageDto
		+ToGarage(GarageDto dto)$ Garage~IVehicle~
	}

	class GarageDto {
		<<record>>
		+int Version
		+int Capacity
		+List~VehicleDto~ Vehicles
	}

	class VehicleDto {
		<<record>>
		+VehicleType Type
		+string RegistrationNumber
		+string Color
		+int NumberOfWheels
		+int ParkingSize
	}

	class ConsoleUI {
		-IHandler _handler
		-TextReader _input
		-TextWriter _output
		-MenuItem[] _menu
		-VehicleTypeInfo[] _vehicleTypes
		+Run() void
	}

	class ConsoleUIText {
		<<static>>
		+const string SearchCriteriaPrompt
		+const string ConfirmLoadOverwrite
	}

	class Program {
		<<static>>
		+Main() void
	}

	IVehicle <|.. Vehicle
	Vehicle <|-- Airplane
	Vehicle <|-- Boat
	Vehicle <|-- Bus
	Vehicle <|-- Car
	Vehicle <|-- Motorcycle
	Vehicle <|-- Truck

	Car --> FuelType
	Vehicle ..> VehicleColors : normalizes color
	SampleData ..> IVehicle : creates
	Garage~T~ o-- IVehicle : stores T
	Garage~T~ ..> SearchCriteria
	Garage~T~ ..> OperationResult
	Garage~T~ ..> VehicleType

	IHandler <|.. GarageHandler
	GarageHandler o-- "0..1" Garage~IVehicle~
	GarageHandler o-- IGarageStorage
	GarageHandler ..> SampleData
	IGarageStorage <|.. JsonGarageStorage
	JsonGarageStorage ..> GarageMapper : uses
	JsonGarageStorage ..> GarageLoadResult : returns
	GarageMapper ..> GarageDto
	GarageDto o-- VehicleDto
	ConsoleUI o-- IHandler
	ConsoleUI ..> ConsoleUIText
	Program ..> ConsoleUI : starts
```
