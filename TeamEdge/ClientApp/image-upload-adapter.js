import store from './store/index'
import axios from 'axios'

class ImageUploadAdapter {
    constructor( loader ) {
        this.loader = loader;
    }

    upload() {
        return this.loader.file
            .then( file => new Promise( ( resolve, reject ) => {
                const data = new FormData();
                data.append( 'file', file );
                axios.post(`/api/file/project/${store.getters['project/project'].id}`,data,
                { headers: {'Content-Type': 'multipart/form-data' }})
                .then(
                    r=>{resolve( {default: r.data.path}); store.commit('createWorkItem/addFileId', r.data.id)},
                    r=>reject( `Couldn't upload file: ${ file.name }.` )
                )
            } ) );
    }
}

export default function(editor) {
    editor.plugins.get( 'FileRepository' ).createUploadAdapter = ( loader ) => {
        return new ImageUploadAdapter( loader );
    };
}