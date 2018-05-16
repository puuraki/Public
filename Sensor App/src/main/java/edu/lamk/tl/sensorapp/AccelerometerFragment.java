package edu.lamk.tl.sensorapp;

import android.Manifest;
import android.content.Context;
import android.content.pm.PackageManager;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.content.ContextCompat;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import java.util.List;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link AccelerometerFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link AccelerometerFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class AccelerometerFragment extends Fragment {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_SECTION_NUMBER = "section_number";

    // TODO: Rename and change types of parameters
    private int mSectionNumber;

    private OnFragmentInteractionListener mListener;

    private SensorManager mSensorManager;
    private SensorEventListener mAccelerometerListener;

    private Sensor mAccelerometer;

    private float[] deltaAccel = new float[] {0, 0, 0};
    private float[] oldAccel = new float[] {0, 0, 0};
    private boolean firstRun = true;
    private boolean allowChanges = false;

    private int accelCounter = 0;

    public AccelerometerFragment() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment AccelerometerFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static AccelerometerFragment newInstance(int sectionNumber) {
        AccelerometerFragment fragment = new AccelerometerFragment();
        Bundle args = new Bundle();
        args.putInt(ARG_SECTION_NUMBER, sectionNumber);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mSectionNumber = getArguments().getInt(ARG_SECTION_NUMBER);
            mSensorManager = (SensorManager) getActivity().getSystemService(Context.SENSOR_SERVICE);

            loadDefaultSensors();

            setAllSensorListeners();
            Log.d( "AccelerometerFragment", "onCreate() AccelerometerFragment");
        }

    }
    public void loadDefaultSensors() {
        mAccelerometer = mSensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER);
    }

    public void setAllSensorListeners() {
        mSensorManager.registerListener(mAccelerometerListener, mAccelerometer, SensorManager.SENSOR_DELAY_NORMAL);
    }
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View rootView = inflater.inflate(R.layout.fragment_accelerometer, container, false);
        return rootView;
    }

    private void updateAccelerometerData( SensorEvent event ) {
        Log.d("AccelerometerFragment", "updateAccelerometerData()");
        TextView tv;

        if (deltaAccel[0] != 0 || firstRun)
        {
            tv = (TextView)getActivity().findViewById( R.id.acc_movX_text );
            tv.setText( String.format("%.3f", event.values[0]) + " m/s²");
        }

        if (deltaAccel[1] != 0 || firstRun)
        {
            tv = (TextView)getActivity().findViewById( R.id.acc_movY_text );
            tv.setText( String.format("%.3f", event.values[1]) + " m/s²");
        }

        if (deltaAccel[2] != 0 || firstRun)
        {
            tv = (TextView)getActivity().findViewById( R.id.acc_movZ_text );
            tv.setText( String.format("%.3f", event.values[2]) + " m/s²");
        }

        tv = (TextView)getActivity().findViewById( R.id.acc_cnt_text );
        tv.setText( Integer.toString(accelCounter) );
        firstRun = false;
    }

    private void startSensorWatch() {
        if (mSensorManager == null) {
            mSensorManager = (SensorManager) getActivity().getSystemService(Context.SENSOR_SERVICE);
            setAllSensorListeners();
            Log.d( "AccelerometerFragment", "startSensorWatch()");
        }
        if( mSensorManager == null ) {
            Log.d( "AccelerometerFragment", "Fatal: mSensorManager is null");
            return;
        }

        mAccelerometerListener = new SensorEventListener() {
            @Override
            public void onSensorChanged(SensorEvent event) {
                deltaAccel[0] = event.values[0] - oldAccel[0];
                deltaAccel[1] = event.values[1] - oldAccel[1];
                deltaAccel[2] = event.values[2] - oldAccel[2];


                oldAccel[0] = event.values[0];
                oldAccel[1] = event.values[1];
                oldAccel[2] = event.values[2];

                if ((deltaAccel[0] >= 0.25 || deltaAccel[1] >= 0.25 || deltaAccel[2] >= 0.25 ||
                        deltaAccel[0] <= -0.25 || deltaAccel[1] <= -0.25 || deltaAccel[2] <= -0.25
                        || firstRun) && allowChanges)
                {
                    Log.d("AccelerometerFragment", "onAccelerometerChanged: " + event.values[0] + " , " + event.values[1] + " , " + event.values[2]);
                    Log.d("AccelerometerFragment", "Delta: " + deltaAccel[0] + " , " + deltaAccel[1] + " , " + deltaAccel[2]);
                    accelCounter++;
                    updateAccelerometerData(event);
                }
            }

            @Override
            public void onAccuracyChanged(Sensor sensor, int accuracy) {

            }
        };
    }
    // TODO: Rename method, update argument and hook method into UI event
    public void onButtonPressed(Uri uri) {
        if (mListener != null) {
            mListener.onFragmentInteraction(uri);
        }
    }

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        startSensorWatch();
    }

    @Override
    public void onDetach() {
        super.onDetach();
        mSensorManager.unregisterListener(mAccelerometerListener);
        mListener = null;
    }

    @Override
    public void setMenuVisibility(final boolean visible){
        super.setMenuVisibility(visible);

        if (visible)
        {
            allowChanges = true;
        }
        else
        {
            allowChanges = false;
        }
    }


    /**
     * This interface must be implemented by activities that contain this
     * fragment to allow an interaction in this fragment to be communicated
     * to the activity and potentially other fragments contained in that
     * activity.
     * <p>
     * See the Android Training lesson <a href=
     * "http://developer.android.com/training/basics/fragments/communicating.html"
     * >Communicating with Other Fragments</a> for more information.
     */
    public interface OnFragmentInteractionListener {
        // TODO: Update argument type and name
        void onFragmentInteraction(Uri uri);
    }
}
