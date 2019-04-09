import store from './store/index'
import axios from 'axios'

class ImageUploadAdapter {
    constructor( loader, callBack) {
        this.loader = loader;
        this.callBack = callBack;
    }

    upload() {
        return this.loader.file
            .then( file => new Promise( ( resolve, reject ) => {
                const data = new FormData();
                data.append( 'file', file );
                axios.post(`/api/file/project/${store.getters['project/project'].id}`,data,
                { headers: {'Content-Type': 'multipart/form-data' }})
                .then(
                    r=>{resolve( {default: r.data.path}); this.callBack(r.data);},
                    r=>reject( `Couldn't upload file: ${ file.name }.` )
                )
            } ) );
    }
}

export default {
    createWIAdapter:editor => {
        editor.plugins.get( 'FileRepository' ).createUploadAdapter = ( loader ) => {
            return new ImageUploadAdapter( loader, data=>store.commit('fileSelector/addFile', data));
        };
    },
    updateWIAdapter:editor=>{
        editor.plugins.get( 'FileRepository' ).createUploadAdapter = ( loader ) => {
            return new ImageUploadAdapter( loader, data=>store.getters['project/currentWI'].changed.files.push(data));
        };
    }
}