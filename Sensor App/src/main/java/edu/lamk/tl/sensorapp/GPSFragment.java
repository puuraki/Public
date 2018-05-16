package edu.lamk.tl.sensorapp;

import android.Manifest;
import android.content.Context;
import android.content.pm.PackageManager;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.ActivityCompat;
import android.support.v4.app.Fragment;
import android.support.v4.content.ContextCompat;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link GPSFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link GPSFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class GPSFragment extends Fragment {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_SECTION_NUMBER = "section_number";

    // TODO: Rename and change types of parameters
    private int mSectionNumber;

    private OnFragmentInteractionListener mListener;

    private LocationManager mLocationManager;
    private LocationListener mLocationListener;

    private double[] deltaLocation = new double[] {0, 0, 0, 0};
    private double[] oldLocation = new double[] {0, 0, 0, 0};


    private int gpsCounter = 0;
    private boolean allowChanges = false;

    public GPSFragment() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param int sectionNumber.
     * @return A new instance of fragment GPSFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static GPSFragment newInstance(int sectionNumber) {
        GPSFragment fragment = new GPSFragment();
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
            Log.d( "GPSFragment", "onCreate() GPSFragment");
        }

        boolean finePermission = ActivityCompat.checkSelfPermission(getActivity(), Manifest.permission.ACCESS_FINE_LOCATION) == PackageManager.PERMISSION_GRANTED;
        boolean coarsePermission = ActivityCompat.checkSelfPermission(getActivity(), Manifest.permission.ACCESS_COARSE_LOCATION) == PackageManager.PERMISSION_GRANTED;

        if(!finePermission)
        {
            ActivityCompat.requestPermissions(getActivity(), new String[]{Manifest.permission.ACCESS_FINE_LOCATION}, 200);
        }

        if(!coarsePermission)
        {
            ActivityCompat.requestPermissions(getActivity(), new String[]{Manifest.permission.ACCESS_COARSE_LOCATION}, 200);
        }

    }

    @Override
    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        switch (requestCode) {
            case 200: {
                if(grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                    // {Some Code}
                }
            }
        }
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View rootView = inflater.inflate(R.layout.fragment_gps, container, false);
        return rootView;
    }

    private void updateGPSLocationData( Location loc ) {
        Log.d("GPSFragment", "updateGPSLocationData()");
        TextView tv = (TextView)getActivity().findViewById( R.id.gps_lat_text );
        tv.setText( String.format("%.6f", loc.getLatitude()));

        tv = (TextView)getActivity().findViewById( R.id.gps_lon_text );
        tv.setText( String.format("%.6f", loc.getLongitude()));

        tv = (TextView)getActivity().findViewById( R.id.gps_alt_text );
        tv.setText( String.format("%.6f", loc.getAltitude()));

        tv = (TextView)getActivity().findViewById( R.id.gps_acc_text );
        tv.setText( String.format("%.6f", loc.getAccuracy()));

        tv = (TextView)getActivity().findViewById( R.id.gps_cnt_text );
        tv.setText( Integer.toString(gpsCounter));
    }
    private void startGPSWatch() {
        if (mLocationManager == null) {
            mLocationManager = (LocationManager) getActivity().getSystemService(Context.LOCATION_SERVICE);
            Log.d( "GPSFragment", "startGPSWatch()");
        }
        if( mLocationManager == null ) {
            Log.d( "GPSFragment", "Fatal: mLocationManger is null");
            return;
        }

        mLocationListener = new LocationListener() {
            @Override
            public void onLocationChanged(Location location) {
                deltaLocation[0] = location.getLatitude() - oldLocation[0];
                deltaLocation[1] = location.getLongitude() - oldLocation[1];
                deltaLocation[2] = location.getAltitude() - oldLocation[2];
                deltaLocation[3] = location.getAccuracy() - oldLocation[3];

                oldLocation[0] = location.getLatitude();
                oldLocation[1] = location.getLongitude();
                oldLocation[2] = location.getAltitude();
                oldLocation[3] = location.getAccuracy();


                Log.d("GPSFragment", "onLocationChanged: " +location.getLatitude() + ", " + location.getLongitude() );
                if ((deltaLocation[0] != 0 || deltaLocation[1] != 0 || deltaLocation[2] != 0 || deltaLocation[3] != 0) && allowChanges) {
                    Log.d("GPSFragment", "onLocationChanged: " +location.getLatitude() + ", " + location.getLongitude() );
                    gpsCounter++;
                    updateGPSLocationData( location );
                }
            }

            @Override
            public void onStatusChanged(String provider, int status, Bundle extras) {
                Log.d("GPSFragment", "onStatusChanged: " + provider + ": " + status);
            }

            @Override
            public void onProviderEnabled(String provider) {

            }

            @Override
            public void onProviderDisabled(String provider) {

            }
        };
        int permissionCoarse = ContextCompat.checkSelfPermission( getActivity(), Manifest.permission.ACCESS_COARSE_LOCATION);
        int permissionFine = ContextCompat.checkSelfPermission( getActivity(), Manifest.permission.ACCESS_FINE_LOCATION);

        if ( permissionCoarse != PackageManager.PERMISSION_GRANTED || permissionFine != PackageManager.PERMISSION_GRANTED) {
            //
            Log.d("GPSFragment", "requestLocationUpdates() failed");
        }
        else {
            mLocationManager.requestLocationUpdates( LocationManager.GPS_PROVIDER, 0,0, mLocationListener );
            Log.d("GPSFragment", "requestLocationUpdates()");
        }
    }

    private void stopGPSWatch() {
        if( mLocationListener == null) {
            Log.d( "GPSFragment", "Fatal: stopGPSWatch() mLocationListener is null");
            return;
        }
        int permissionCoarse = ContextCompat.checkSelfPermission( getActivity(), Manifest.permission.ACCESS_COARSE_LOCATION);
        int permissionFine = ContextCompat.checkSelfPermission( getActivity(), Manifest.permission.ACCESS_FINE_LOCATION);

        if ( permissionCoarse != PackageManager.PERMISSION_GRANTED || permissionFine != PackageManager.PERMISSION_GRANTED) {
            //
            Log.d("GPSFragment", "removeUpdates() failed");
        }
        else {
            mLocationManager.removeUpdates( mLocationListener );
            Log.d("GPSFragment", "removeUpdates()");
        }
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
        Log.d("GPSFragment", "onAttach()");
        if (context instanceof OnFragmentInteractionListener) {
            mListener = (OnFragmentInteractionListener) context;
        } else {
            throw new RuntimeException(context.toString()
                    + " must implement OnFragmentInteractionListener");
        }
        startGPSWatch();
    }

    @Override
    public void onDetach() {
        super.onDetach();
        mListener = null;
        stopGPSWatch();
        Log.d("GPSFragment", "onDetach()");
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
