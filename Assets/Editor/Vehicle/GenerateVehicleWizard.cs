using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Utilities;
using Vehicle.Data;

namespace EditorSpecific.Vehicle
{
    public class GenerateVehicleWizard : ScriptableWizard
    {
        [SerializeField] private GameObject _carPrefab;
        [SerializeField] private CarEditorData carEditorData;
        [SerializeField] private VehicleSo _vehicleSo;

        private GameObject _generatedCar;
        private bool _hasError;
        private string _errorMessage;

        [MenuItem("Tools/GenerateVehicle")]
        public static void CreateWizard()
        {
            DisplayWizard<GenerateVehicleWizard>("GenerateVehicle", "CREATE", "SAVE");
        }

        private void OnEnable()
        {
            TryFindVehicleParts();
        }

        private void OnValidate()
        {
            TryFindVehicleParts();
        }

        public void OnWizardCreate()
        {
            if (!_generatedCar)
            {
                Debug.Log("Something went wrong");
                return;
            }
            foreach (var (transformName, go) in carEditorData.NameObjDict)
            {
                var wheelCollider = new GameObject($"Collider{transformName}").AddComponent<Wheel>();
                wheelCollider.transform.SetParent(go.transform);
                wheelCollider.transform.localPosition = Vector3.zero;
            }

            var vehicleEngine = _generatedCar.AddComponent<VehicleEngine>();
            vehicleEngine.SetRigidBody(_generatedCar.AddComponent<Rigidbody>());
            vehicleEngine.SetVehicleData(_vehicleSo);
            var wheels = vehicleEngine.gameObject.AddComponent<VehicleWheels>();
            wheels.BackLeft = carEditorData.NameObjDict[carEditorData.CarBLObjName].GetComponentInChildren<Wheel>();
            wheels.BackRight = carEditorData.NameObjDict[carEditorData.CarBRObjName].GetComponentInChildren<Wheel>();
            wheels.FrontLeft = carEditorData.NameObjDict[carEditorData.CarFlObjName].GetComponentInChildren<Wheel>();
            wheels.FrontRight = carEditorData.NameObjDict[carEditorData.CarFrObjName].GetComponentInChildren<Wheel>();
            vehicleEngine.SetWheels(wheels);
        }

        public void OnWizardUpdate()
        {
            errorString = _hasError ? _errorMessage : "";
        }

        public void OnWizardOtherButton()
        {
        }

        private bool TryFindVehicleParts()
        {
            if (_carPrefab == null)
            {
                _hasError = true;
                _errorMessage = "Car prefab is empty, please pick a prefab";
                return false;
            }
            
            _generatedCar = Instantiate(_carPrefab);
            carEditorData.InitializeCarData();
            var nameAndTransformList = _generatedCar.transform.TraverseTransformTree(child =>
            {
                return string.Equals(child.name, carEditorData.CarFlObjName, StringComparison.OrdinalIgnoreCase) ||
                       string.Equals(child.name, carEditorData.CarFrObjName, StringComparison.OrdinalIgnoreCase) ||
                       string.Equals(child.name, carEditorData.CarBLObjName, StringComparison.OrdinalIgnoreCase) ||
                       string.Equals(child.name, carEditorData.CarBRObjName, StringComparison.OrdinalIgnoreCase);
            },(child) => child.name);

            foreach (var (transformName, transform) in nameAndTransformList)
            {
                if (carEditorData.NameObjDict.TryGetValue(transformName,out var value))
                {
                    carEditorData.NameObjDict[transformName] = transform.gameObject;
                }
            }

            if (carEditorData.HasMissingParts())
            {
                _hasError = true;
                _errorMessage = "Car prefab has missing parts";
                return false;
            }

            _hasError = false;
            return true;
        }

        [System.Serializable]
        private class CarEditorData
        {
            [SerializeField] private string _carFLObjName = "Wheel_FL";
            [SerializeField] private string _carFRObjName = "Wheel_FR";
            [SerializeField] private string _carBLObjName = "Wheel_BL";
            [SerializeField] private string _carBRObjName = "Wheel_BR";
            
            public Dictionary<string, GameObject> NameObjDict { get; private set; }

            public GameObject CarFlObj { get; set; }
            public GameObject CarFrObj { get; set; }
            public GameObject CarBlObj { get; set; }
            public GameObject CarBrObj { get; set; }

            public string CarFlObjName => _carFLObjName;
            public string CarFrObjName => _carFRObjName;
            public string CarBLObjName => _carBLObjName;
            public string CarBRObjName => _carBRObjName;

            public void InitializeCarData()
            {
                NameObjDict = new Dictionary<string, GameObject>(4);
                NameObjDict.Add(_carFLObjName,null);
                NameObjDict.Add(_carFRObjName,null);
                NameObjDict.Add(_carBLObjName,null);
                NameObjDict.Add(_carBRObjName,null);
            }
            
            public bool HasMissingParts()
            {
                return NameObjDict.Values.Any(value => value == null);
            }
        }
    }
}