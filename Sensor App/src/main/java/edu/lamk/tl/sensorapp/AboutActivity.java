package edu.lamk.tl.sensorapp;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

public class AboutActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_about);

        String developer = getIntent().getStringExtra("DEVELOPER");
        // logcat debugging
        Log.d("AboutActivity", "onCreate called with message: " + developer);

        TextView tv = (TextView)findViewById( R.id.about_dev_name );
        tv.setText( developer );

        Button backButton = (Button) findViewById(R.id.backButton);
        backButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Log.d("AboutActivity", "BackButton clicked");
                Intent intent = getIntent();
                intent.putExtra("MY_RETURN_VALUE", "100");
                setResult(RESULT_OK, intent );
                finish();
            }
        });
    }
}
