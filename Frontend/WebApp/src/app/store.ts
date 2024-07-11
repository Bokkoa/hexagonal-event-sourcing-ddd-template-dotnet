import { configureStore } from '@reduxjs/toolkit';
import { combineReducers } from 'redux';
import persistReducer from 'redux-persist/es/persistReducer';
import persistStore from 'redux-persist/es/persistStore';
import storage from 'redux-persist/lib/storage';

const rootReducer = combineReducers({

});

const persistConfig = {
  key: 'root',
  storage, 
  whitelist: [],
}
const persistedReducer = persistReducer(persistConfig, rootReducer);

const store = configureStore({
  reducer: persistedReducer
})

export const persistor = persistStore(store);
export default store;
