/* global device, google */

var app = {

     // deviceData stores device info
    deviceData: {
        cordova:        '0',
        model:          '0',
        platform:       '0',
        uuid:           '0',
        version:        '0',
        manufacturer:   '0',
        serial:         '0'
    },
    
    // compassData stores options and update and error counter for compass along 
    // with the ID of the compass watch
    compassData: {
        watchId: -1,
        dataCounter: 0,
        errorCounter: 0,
        options: {
            frequency: 1000
        }
    },
    
    // Like compassData, but also stores values (latitude and longitude)from sensor
    gpsData: {
      watchId: -1,
      dataCounter: 0,
      errorCounter: 0,
      latitude: 0,
      longitude: 0,
      options: {
          enableHighAccuracy: true,
          maximumAge: 1000
      }
    },
    
    // options for Google map
    mapOptions: {
        center: new google.maps.LatLng(0,0),
        zoom: 1,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    },
    
    /* 
     * mapData contains boolean just to check if marker is already set
     * along with the map and marker objects
     */
    mapData: {
        isMarkerSet: false,
        map: new google.maps.Map(document.getElementById("map"), this.mapOptions),
        marker: new google.maps.Marker()
    },
        
    // Application Constructor
    initialize: function() {
        document.addEventListener('deviceready', this.onDeviceReady.bind(this), false);
    },

    // deviceready Event Handler
    //
    // Bind any cordova events here. Common events are:
    // 'pause', 'resume', etc.
    onDeviceReady: function() {
        this.setDeviceData();
        this.showDeviceData();
        this.initializeCompassWatch();
        this.initializeGpsWatch();
        this.initializeMap();
    },
    
    setDeviceData: function() {
        this.deviceData.cordova = device.cordova;
        this.deviceData.model = device.model;
        this.deviceData.platform = device.platform;
        this.deviceData.uuid = device.uuid;
        this.deviceData.version = device.version;
        this.deviceData.manufacturer = device.manufacturer;
        this.deviceData.serial = device.serial;
    },
    
    // Update device data
    showDeviceData: function() {
        var e = document.getElementById('device-cordova');
        e.innerHTML = this.deviceData.cordova;
        
        e = document.getElementById('device-model');
        e.innerHTML = this.deviceData.model;
        
        e = document.getElementById('device-platform');
        e.innerHTML = this.deviceData.platform;
        
        e = document.getElementById('device-uuid');
        e.innerHTML = this.deviceData.uuid;
        
        e = document.getElementById('device-version');
        e.innerHTML = this.deviceData.version;
        
        e = document.getElementById('device-manufacturer');
        e.innerHTML = this.deviceData.manufacturer;
        
        e = document.getElementById('device-serial');
        e.innerHTML = this.deviceData.serial;
    },
    
    // Compass handling
    initializeCompassWatch: function() {
        if(this.compassData.watchId !== -1){
           this.removeCompassWatch();
        }
        this.compassData.watchId = navigator.compass.watchHeading(
                this.onCompassSuccess, 
                this.onCompassError, 
                this.compassData.options);
        console.log("Compass watch initialized: " + this.compassData.watchId);
    },
    
    removeCompassWatch: function() {
        if(this.compassData.watchId !== -1) {
            navigator.compass.clearWatch(this.compassData.watchId);
            this.compassData.watchId = -1;
        }
    },
    
    // Update compass data
    onCompassSuccess: function(heading) {
        app.compassData.dataCounter++;

        var e = document.getElementById('compass-magnetic-heading');
        e.innerHTML = Number(heading.magneticHeading).toFixed(4);
        
        e = document.getElementById('compass-true-heading');
        e.innerHTML = Number(heading.trueHeading).toFixed(4);
        
        e = document.getElementById('compass-accuracy');
        e.innerHTML = Number(heading.headingAccuracy).toFixed(2);
        
        e = document.getElementById('compass-counter');
        e.innerHTML = app.compassData.dataCounter;
    },
    
    onCompassError: function(err) {
        this.compassData.errorCounter++;
        console.log("onCompassError: " + err.code);
    },
    
    // GPS handling
    initializeGpsWatch: function() {
        if(this.gpsData.watchId !== -1){
            this.removeGpsWatch();
        }
        this.gpsData.watchId = navigator.geolocation.watchPosition(
            this.onGpsSuccess,
            this.onGpsError,
            this.gpsData.options);
        console.log("GPS watch initialized: " + this.gpsData.watchId);
    },
    
    removeGpsWatch: function() {
        if(this.gpsData.watchId !== -1){
            navigator.geolocation.clearWatch(this.gpsData.watchId);
            this.gpsData.watchId = -1;
        }
    },
    
    initializeMap: function(){        
        this.mapData.map.setZoom(15); 
    },
    
    // Set marker and update its position
    setMarker: function(latitude, longitude) {
        google.maps.event.trigger(this.mapData.map, 'resize');
        var latLong = new google.maps.LatLng(latitude, longitude);
        
        if(!this.mapData.isMarkerSet){
            this.mapData.marker.setPosition(latLong);
            this.mapData.marker.setMap(this.mapData.map);
            this.mapData.isMarkerSet = true;
        }
        else{
            this.mapData.marker.setPosition(latLong);
        }
        
        this.mapData.map.panTo(this.mapData.marker.getPosition());
    },
    
    // Set GPS data and update position for map marker
    onGpsSuccess: function(position) {
        app.gpsData.dataCounter++;
        
        var updatedLatitude = position.coords.latitude;
        var updatedLongitude = position.coords.longitude;
        
        var e = document.getElementById('gps-latitude');
        e.innerHTML = Number(updatedLatitude).toFixed(4);
        
        e = document.getElementById('gps-longitude');
        e.innerHTML = Number(updatedLongitude).toFixed(4);
        
        e = document.getElementById('gps-accuracy');
        e.innerHTML = Number(position.coords.accuracy).toFixed(4);
        
        e = document.getElementById('gps-altitude');
        e.innerHTML = Number(position.coords.altitude).toFixed(4);
        
        e = document.getElementById('gps-alt-accuracy');
        e.innerHTML = Number(position.coords.altitudeAccuracy).toFixed(4);
        
        e = document.getElementById('gps-speed');
        e.innerHTML = Number(position.coords.speed).toFixed(4);
        
        e = document.getElementById('gps-counter');
        e.innerHTML = app.gpsData.dataCounter;   
        
        if (updatedLatitude !== app.gpsData.latitude || updatedLongitude !== app.gpsData.longitude) {

            app.gpsData.latitude = position.coords.latitude;
            app.gpsData.longitude = position.coords.longitude;

            app.setMarker(updatedLatitude, updatedLongitude);
        }
    },
    
    onGpsError: function(err) {
        this.compassData.errorCounter++
        console.log("onGpsError: " + err.code);
    }
    
};

app.initialize();