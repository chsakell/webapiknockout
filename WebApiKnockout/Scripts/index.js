/// <reference path="knockout-2.3.0.js" />
/// <reference path="jquery-2.0.3.js" />

var firstAlbumId = 0;

function Artist(artistId, artistName) {
    var self = this;
    self.ArtistId = ko.observable(artistId);
    self.Name = ko.observable(artistName);
    self.ArtistAlbumsUrl = ko.computed(function () {
        return "../api/albums?artistId=" + this.ArtistId();
    }, this);
}

function Album() {
    var self = this;
    self.AlbumId = ko.observable();
    self.Title = ko.observable("");
    self.ArtistId = ko.observable();
}

function Genre(genreId, name) {
    var self = this;
    self.GenreId = ko.observable(genreId);
    self.Name = ko.observable(name)
}

function Track(trackId, name, genre, milliseconds, unitPrice) {
    // Properties
    var self = this;
    self.TrackId = ko.observable(trackId);
    self.Name = ko.observable(name);
    self.Genre = ko.observable(genre);
    self.Milliseconds = ko.observable(milliseconds);
    self.UnitPrice = ko.observable(unitPrice);
    self.FormattedPrice = ko.computed(function () {
        return self.UnitPrice() + " $";
    }, this);
    self.CurrentTemplate = ko.observable("displayTrack");

    // Functions
    self.editTrack = function () {
        self.CurrentTemplate("editTrack");
    }
    self.cancelEditTrack = function () {
        self.CurrentTemplate("displayTrack");
    }
    self.updateTrack = function () {
        var updatedTrack = {
            TrackId: self.TrackId(),
            Name: self.Name(),
            Genre: self.Genre(),
            Milliseconds: self.Milliseconds(),
            UnitPrice: self.UnitPrice()
        };
        $.ajax({
            type: "PUT",
            url: "/api/tracks/"+self.TrackId(),
            dataType: "json",
            data: updatedTrack,
            success: function () {
                $('#divTrackUpdated').slideDown(3000);
                $('#divTrackUpdated').slideUp(500);
                self.CurrentTemplate("displayTrack");
            },
            error: function () {
                alert('Error while trying to update track..');
            }
        });
        
    }
}

function ChinookViewModel() {
    // Properties
    var self = this;
    this.ArtistsSearched = ko.observableArray([]);
    this.ArtistAlbumsSearched = ko.observableArray([]);
    this.AlbumTracks = ko.observableArray([]);
    this.Genres = ko.observableArray([]);

    // Functions
    this.searchArtists = function () {
        //$('#tblSearchArtistAlbums').css('visibility', 'hidden');
        $('#divSelectArtistAlbums').css('visibility', 'hidden');
        $('#tblSelectedAlbumTracks').css('visibility', 'hidden');
        var name = $('#inputSearchArtist').val();
        $('#tbodySearchArtists').empty();
        $.getJSON("/api/artists?name=" + name, function (artists) {
            $.each(artists, function (index, artist) {
                self.ArtistsSearched.push(new Artist(artist.ArtistId, artist.Name));
            });
        });
        $('#tblSearchArtists').css('visibility', 'visible');
    };

    this.showArtistAlbums = function (artist) {
        self.ArtistAlbumsSearched([]);
        $('#tbodySearchArtistAlbum').empty();
        $.getJSON(artist.ArtistAlbumsUrl(), function (albums) {
            $.each(albums, function (index, album) {
                self.ArtistAlbumsSearched.push(album);
            });
            firstAlbumId = albums[0].AlbumId;
        }).done(function () { self.showAlbumTracks(); });
        $('#divSelectArtistAlbums').css('visibility', 'visible');

    };

    this.showAlbumTracks = function (data, event) {
        if (data == null) {
            data = firstAlbumId;
        }
        $('#tblSelectedAlbumTracks').css('visibility', 'visible');
        self.AlbumTracks([]);
        $.getJSON('/api/tracks?albumId=' + data, function (tracks) {
            $.each(tracks, function (index, track) {
                self.AlbumTracks.push(new Track(track.TrackId, track.Name, track.Genre, track.Milliseconds, track.UnitPrice));
            });
        });
    }
}

var vm = new ChinookViewModel();
// Initialize Genres
$.getJSON('/api/genres/', function (genres) {
    $.each(genres, function (index, genre) {
        vm.Genres.push(genre);
    });

})
ko.applyBindings(vm);