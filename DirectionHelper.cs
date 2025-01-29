using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TransportSystemClient.Models
{
    public class DirectionHelper
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class GeocodedWaypoint
        {
            [JsonProperty("geocoder_status")]
            public string GeocoderStatus { get; set; }

            [JsonProperty("place_id")]
            public string PlaceId { get; set; }

            [JsonProperty("types")]
            public List<string> Types { get; set; }
        }

        public class Northeast
        {
            [JsonProperty("lat")]
            public double Lat { get; set; }

            [JsonProperty("lng")]
            public double Lng { get; set; }
        }

        public class Southwest
        {
            [JsonProperty("lat")]
            public double Lat { get; set; }

            [JsonProperty("lng")]
            public double Lng { get; set; }
        }

        public class Bounds
        {
            [JsonProperty("northeast")]
            public Northeast Northeast { get; set; }

            [JsonProperty("southwest")]
            public Southwest Southwest { get; set; }
        }

        public class Distance
        {
            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("value")]
            public int Value { get; set; }
        }

        public class Duration
        {
            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("value")]
            public int Value { get; set; }
        }

        public class EndLocation
        {
            [JsonProperty("lat")]
            public double Lat { get; set; }

            [JsonProperty("lng")]
            public double Lng { get; set; }
        }

        public class StartLocation
        {
            [JsonProperty("lat")]
            public double Lat { get; set; }

            [JsonProperty("lng")]
            public double Lng { get; set; }
        }

        public class Polyline
        {
            [JsonProperty("points")]
            public string Points { get; set; }
        }

        public class Step
        {
            [JsonProperty("distance")]
            public Distance Distance { get; set; }

            [JsonProperty("duration")]
            public Duration Duration { get; set; }

            [JsonProperty("end_location")]
            public EndLocation EndLocation { get; set; }

            [JsonProperty("html_instructions")]
            public string HtmlInstructions { get; set; }

            [JsonProperty("polyline")]
            public Polyline Polyline { get; set; }

            [JsonProperty("start_location")]
            public StartLocation StartLocation { get; set; }

            [JsonProperty("travel_mode")]
            public string TravelMode { get; set; }

            [JsonProperty("maneuver")]
            public string Maneuver { get; set; }
        }

        public class Leg
        {
            [JsonProperty("distance")]
            public Distance Distance { get; set; }

            [JsonProperty("duration")]
            public Duration Duration { get; set; }

            [JsonProperty("end_address")]
            public string EndAddress { get; set; }

            [JsonProperty("end_location")]
            public EndLocation EndLocation { get; set; }

            [JsonProperty("start_address")]
            public string StartAddress { get; set; }

            [JsonProperty("start_location")]
            public StartLocation StartLocation { get; set; }

            [JsonProperty("steps")]
            public List<Step> Steps { get; set; }

            [JsonProperty("traffic_speed_entry")]
            public List<object> TrafficSpeedEntry { get; set; }

            [JsonProperty("via_waypoint")]
            public List<object> ViaWaypoint { get; set; }
        }

        public class OverviewPolyline
        {
            [JsonProperty("points")]
            public string Points { get; set; }
        }

        public class Route
        {
            [JsonProperty("bounds")]
            public Bounds Bounds { get; set; }

            [JsonProperty("copyrights")]
            public string Copyrights { get; set; }

            [JsonProperty("legs")]
            public List<Leg> Legs { get; set; }

            [JsonProperty("overview_polyline")]
            public OverviewPolyline OverviewPolyline { get; set; }

            [JsonProperty("summary")]
            public string Summary { get; set; }

            [JsonProperty("warnings")]
            public List<object> Warnings { get; set; }

            [JsonProperty("waypoint_order")]
            public List<object> WaypointOrder { get; set; }
        }

        public class Directionsmodel
        {
            [JsonProperty("geocoded_waypoints")]
            public List<GeocodedWaypoint> GeocodedWaypoints { get; set; }

            [JsonProperty("routes")]
            public List<Route> Routes { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }
        }

    }
}
