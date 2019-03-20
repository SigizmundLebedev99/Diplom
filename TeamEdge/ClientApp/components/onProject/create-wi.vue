<template>
    <v-dialog v-model="dialog" :width="ofSize({xs:'95%', sm:'70%', md:'60%'})" scrollable>
        <v-card>
            <v-card-title class="primary white--text">
                <span class="title">Создание единицы работы</span>
            </v-card-title>
            <v-card-text>
                <v-form>
                <v-text-field v-model="model.name"></v-text-field>
                <ckeditor :editor="editor" v-model="editorData" :config="editorConfig"></ckeditor>
                </v-form>
            </v-card-text>
            <v-card-actions>
                <v-btn @click="alert()">Alert</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
import {mapGetters, mapMutations} from 'vuex'
import onResize from '../../mixins/on-resize'
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import MyCustomUploadAdapterPlugin from '../../image-upload-adapter'
export default {
    mixins:[onResize],
    data:()=>({
        model:{
            name:null
        },
        editor: ClassicEditor,
        editorData: '',
        editorConfig: {
            extraPlugins: [ MyCustomUploadAdapterPlugin ]
        }
    }),
    methods:{
        ...mapMutations({setDialog:'createWorkItem/setDialog'}),
        alert(){
            console.log(this.editorData);
        }
    },
    computed:{
        dialog:{
            ...mapGetters({get:'createWorkItem/dialog'}),
            set(value){
                this.setDialog(value);
            }
        }
    }
}
</script>

