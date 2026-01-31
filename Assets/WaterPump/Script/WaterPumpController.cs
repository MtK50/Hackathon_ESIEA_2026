using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public enum PumpState { Stop, UnFill, Fill, Both }

public enum MethodControl { State, FillValue, PressureValue, PulsePattern }

[RequireComponent(typeof(BluetoothManager))]
public class WaterPumpController : MonoBehaviour
{
    private BluetoothManager m_BluetoohManager;

    [Header("Connection")]

    [SerializeField] private bool m_ResetConnection = false;

    [Header("Configuration")]

    [SerializeField] private MethodControl m_MethodControl;

    [SerializeField] private bool m_EmergencyReset = false;
    public bool EmergencyReset
    {
        get { return m_EmergencyReset; }
        set { m_EmergencyReset = value; }
    }

    [SerializeField] private bool m_ResetSystemDefault = false;
    public bool ResetSystemDefault
    {
        get { return m_ResetSystemDefault; }
        set { m_ResetSystemDefault = value; }
    }

    [SerializeField] private float m_ResetUnFillTime = 10;

    public float ResetUnFillTime
    {
        get { return m_ResetUnFillTime; }
        set { m_ResetUnFillTime = value; }
    }

    [Header("State Mode Settings")]

    [SerializeField] private PumpState m_CurrentState;

    private PumpState m_LastState;

    [Header("Fill Mode Settings")]

    private float m_MaxFill = 100f;

    [Header("Fill Mode Settings")]

    [Tooltip("Max fill time in second")][SerializeField] private float m_FillMaxTime = 20f;

    [Tooltip("Speed in percentage per second")][SerializeField] private float m_Speed;

    [SerializeField][Range(0, 100)] private float m_CurrentFill = 0;

    [Tooltip("Fill value between 0 and 100 %")][Range(0, 100)][SerializeField] private float m_TargetFill = 0;

    public float FillMaxTime
    {
        get { return m_FillMaxTime; }
    }

    public float Speed
    {
        get { return m_Speed; }
    }

    public float CurrentFill
    {
        get { return m_CurrentFill; }
    }

    public float TargetFill
    {
        get { return m_TargetFill; }
        set { m_TargetFill = value; }
    }

    [Header("Pressure Mode Settings")]

    [Tooltip("Range Threshold Pressure Value")][SerializeField] private float m_RangeThreshold = 2;
    [Tooltip("Curent Pressure Value")][Range(0, 100)][SerializeField] private float m_CurrentPressure;
    [Tooltip("Target Pressure Sensor Value")][Range(0, 100)][SerializeField] private float m_TargetPressure;

    [Header("Pulse Pattern Settings")]

    [SerializeField] private float m_PulseFillTime = 0.5f;
    [SerializeField] private float m_PulseUnFillTime = 0.5f;
    [SerializeField] private bool m_PulseEnable;

    private bool m_IsPulsing;
    private float m_PulseTimer;


    [Header("Calibration")]

    [SerializeField] private bool m_CalibrateSystemAtStart = false;
    [SerializeField] private bool m_StartCalibration = false;
    [Tooltip("Unfill Start delay before calibrate")][SerializeField] private float m_UnFillTime = 5.0f;
    [Tooltip("Treshold Value to calibrate the system between 0 and 100")][Range(0, 100)][SerializeField] private float m_CalibrationPressureThreshold = 1;

    private int m_Direction = 0;


    private bool m_WaterPumpConnected = false;

    private bool m_SystemCalibrated = false;


    public bool IsSystemReady
    {
        get { return (m_WaterPumpConnected && m_SystemCalibrated); }
    }


    private void OnValidate()
    {
        m_Speed = (m_MaxFill / m_FillMaxTime);
    }

    private void Start()
    {
        m_BluetoohManager = GetComponent<BluetoothManager>();

        m_BluetoohManager.OnConnectedEvent.AddListener(WaterPumpConnected);
        m_BluetoohManager.OnDataReceiveEvent.AddListener(OnWeightSensorDate);
    }

    private void WaterPumpConnected()
    {
        m_WaterPumpConnected = true;

        m_ResetSystemDefault = true;

        if (m_CalibrateSystemAtStart)
        {
            StartCoroutine(CalibrateSystem());
        }
    }

    IEnumerator CalibrateSystem()
    {
        m_CurrentState = PumpState.UnFill;
        yield return new WaitForSeconds(m_UnFillTime);

        m_CurrentState = PumpState.Fill;

        while (m_CurrentPressure < m_CalibrationPressureThreshold)
        {
            yield return null;
        }

        m_CurrentState = PumpState.Stop;
        m_SystemCalibrated = true;
    }

    private void OnWeightSensorDate(string data)
    {
        m_CurrentPressure = (float)(int.Parse(data)) / 10.0f;
    }


    IEnumerator SystemDefault()
    {
        m_CurrentState = PumpState.UnFill;

        m_TargetFill = 0;
        m_CurrentFill = 0;

        m_TargetPressure = 0;

        yield return new WaitForSeconds(m_ResetUnFillTime);

        m_CurrentState = PumpState.Stop;

        if (m_CalibrateSystemAtStart)
        {
            StartCoroutine(CalibrateSystem());
        }
        else
        {
            m_SystemCalibrated = true;
        }
    }

    IEnumerator Reconnect()
    {
        m_BluetoohManager.Disconnect();
        yield return new WaitForSeconds(1);
        m_BluetoohManager.TryToConnect();
    }

    void Update()
    {
        if (m_ResetConnection)
        {
            m_WaterPumpConnected = false;
            m_ResetConnection = false;
            StartCoroutine(Reconnect());
        }

        if (m_EmergencyReset)
        {
            m_MethodControl = MethodControl.State;
            m_CurrentState = PumpState.Stop;
            m_BluetoohManager.OnDataSendEvent.Invoke("O");
            m_TargetFill = 0;
            m_CurrentFill = 0;
            m_EmergencyReset = false;
        }

        if (m_StartCalibration)
        {
            m_SystemCalibrated = false;
            m_StartCalibration = false;
            StartCoroutine(CalibrateSystem());

        }

        if (m_ResetSystemDefault)
        {
            m_SystemCalibrated = false;
            m_ResetSystemDefault = false;
            StartCoroutine(SystemDefault());
        }

        if (!m_WaterPumpConnected)
            return;

        if (m_MethodControl == MethodControl.FillValue && m_SystemCalibrated)
        {
            m_CurrentFill += m_Speed * m_Direction * Time.deltaTime;

            float currentFill = Mathf.Floor(m_CurrentFill * 10f) / 10f;

            float targetFill = Mathf.Floor(m_TargetFill * 10f) / 10f;

            if (currentFill < targetFill)
            {
                m_CurrentState = PumpState.Fill;
                m_Direction = 1;
            }
            else if (currentFill > targetFill)
            {
                m_CurrentState = PumpState.UnFill;
                m_Direction = -1;
            }
            else if (currentFill == targetFill)
            {
                m_CurrentState = PumpState.Stop;
                m_Direction = 0;
            }
        }
        else if (m_MethodControl == MethodControl.PressureValue && m_SystemCalibrated)
        {

            if ((int)m_CurrentPressure < m_TargetPressure - m_RangeThreshold)
            {
                m_CurrentState = PumpState.Fill;
                m_Direction = 1;
            }
            else if ((int)m_CurrentPressure > m_TargetPressure + m_RangeThreshold)
            {
                m_CurrentState = PumpState.UnFill;
                m_Direction = -1;
            }
            else if ((int)m_CurrentPressure > m_TargetPressure - m_RangeThreshold && (int)m_CurrentPressure < m_TargetPressure + m_RangeThreshold)
            {
                m_CurrentState = PumpState.Stop;
                m_Direction = 0;
            }
        }
        else if (m_MethodControl == MethodControl.PulsePattern && m_SystemCalibrated)
        {
            if(m_PulseEnable)
            {
                if(!m_IsPulsing)
                {
                    m_CurrentState = PumpState.Fill;
                }

                m_IsPulsing = true;

                m_PulseTimer += Time.deltaTime;

                switch (m_CurrentState)
                {
                    case PumpState.Fill:
                        if (m_PulseTimer >= m_PulseFillTime)
                        {
                            m_CurrentState = PumpState.UnFill;
                            m_PulseTimer = 0f;
                        }
                        break;

                    case PumpState.UnFill:
                        if (m_PulseTimer >= m_PulseUnFillTime)
                        {
                            m_CurrentState = PumpState.Fill;
                            m_PulseTimer = 0f;
                        }
                        break;
                }
            }
            else if (!m_PulseEnable)
            {
                m_IsPulsing = false;
                m_CurrentState = PumpState.Stop;
                m_PulseTimer = 0;
            }
        }

        switch (m_CurrentState)
        {
            case PumpState.Stop:
                if (m_LastState != m_CurrentState)
                {
                    m_BluetoohManager.OnDataSendEvent.Invoke("O");
                    m_LastState = m_CurrentState;
                }
                break;
            case PumpState.UnFill:
                if (m_LastState != m_CurrentState)
                {
                    m_BluetoohManager.OnDataSendEvent.Invoke("F");
                    m_LastState = m_CurrentState;
                }
                break;
            case PumpState.Fill:
                if (m_LastState != m_CurrentState)
                {
                    m_BluetoohManager.OnDataSendEvent.Invoke("S");
                    m_LastState = m_CurrentState;
                }
                break;
            case PumpState.Both:
                if (m_LastState != m_CurrentState)
                {
                    m_BluetoohManager.OnDataSendEvent.Invoke("B");
                    m_LastState = m_CurrentState;
                }
                break;
            default:
                break;
        }
    }

    public void ManualControl(PumpState PumpState)
    {
        m_MethodControl = MethodControl.State;
        m_CurrentState = PumpState;
    }

    public void Pulse(bool state)
    {
        m_MethodControl = MethodControl.PulsePattern;
        m_PulseEnable = state;
        m_IsPulsing = false;
    }

    public void PulseSettings(float pulseFillTime, float pulseUnFillTime)
    {
        m_PulseFillTime = pulseFillTime;
        m_PulseUnFillTime = pulseUnFillTime;
    }

    public void Fill(bool state)
    {
        if(state)
        {
            m_MethodControl = MethodControl.FillValue;
        }
        else
        {
            m_MethodControl = MethodControl.State;
            m_CurrentState = PumpState.Stop;
        }

    }

    public void FillSettings(float targetFill)
    {
        m_TargetFill = targetFill;
    }
}
