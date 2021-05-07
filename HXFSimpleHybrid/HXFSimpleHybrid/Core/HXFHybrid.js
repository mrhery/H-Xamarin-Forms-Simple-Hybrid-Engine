/*
 * 
 * Hybrid JS -  A script to bridging the JS with the C# Native API
 * 
 * */
function HXFRequestToNative() {
    
}

let HXF = {
    hardware: {
        camera: {
            init: function () {
                HXFInvoke("permission:camera");
            }
        },
        microphone: {
            init: function () {
                HXFInvoke("permission:microphone");
            }
        }
    }
};