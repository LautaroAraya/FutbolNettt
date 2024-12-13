using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FutbolNet.Class
{
    public abstract class NotificationObject : INotifyPropertyChanged
    {
        // Evento para notificar cambios en las propiedades
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifica cuando una propiedad cambia de valor.
        /// </summary>
        /// <param name="propertyName">El nombre de la propiedad que cambió.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Método helper para establecer un nuevo valor a una propiedad y notificar cambios.
        /// </summary>
        /// <typeparam name="T">Tipo de la propiedad.</typeparam>
        /// <param name="field">Campo backing de la propiedad.</param>
        /// <param name="value">Nuevo valor que se desea establecer.</param>
        /// <param name="propertyName">Nombre de la propiedad. Se obtiene automáticamente si no se proporciona.</param>
        /// <returns>True si el valor fue cambiado; False si no hubo cambios.</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            // Compara el nuevo valor con el actual
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            // Actualiza el campo backing
            field = value;

            // Notifica el cambio
            OnPropertyChanged(propertyName);

            return true;
        }
    }
}
