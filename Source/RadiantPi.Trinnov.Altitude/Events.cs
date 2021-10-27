/*
 * RadiantPi.Trinnov.Altitude - Communication client for Trinnov Altitude
 * Copyright (C) 2020-2021 - Steve G. Bjorg
 *
 * This program is free software: you can redistribute it and/or modify it
 * under the terms of the GNU Affero General Public License as published by the
 * Free Software Foundation, either version 3 of the License, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 * FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more
 * details.
 *
 * You should have received a copy of the GNU Affero General Public License along
 * with this program. If not, see <https://www.gnu.org/licenses/>.
 */

using System;

namespace RadiantPi.Trinnov.Altitude {
    public sealed class AudioDecoderChangedEventArgs : EventArgs {

        //--- Constructors ---
        public AudioDecoderChangedEventArgs(string decoder, string upmixer) {
            Decoder = decoder ?? throw new ArgumentNullException(nameof(decoder));
            Upmixer = upmixer ?? throw new ArgumentNullException(nameof(upmixer));
        }

        //--- Properties ---
        public string Decoder { get; }
        public string Upmixer { get; }
    }

    public sealed class VolumeChangedEventArgs : EventArgs {

        //--- Constructors ---
        public VolumeChangedEventArgs(float volume) => Volume = volume;

        //--- Properties ---
        public float Volume { get; init; }
    }

    public sealed class MuteChangedEventArgs : EventArgs {

        //--- Constructors ---
        public MuteChangedEventArgs(bool muted) => Muted = muted;

        //--- Properties ---
        public bool Muted { get; }
    }
}