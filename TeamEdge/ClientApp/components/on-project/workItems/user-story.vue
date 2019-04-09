<template>
    <div>
        <v-container v-if="loading && number">
            <v-layout column justify-center align-center fill-height>
                <v-progress-circular indeterminate color="primary"></v-progress-circular>
            </v-layout>
        </v-container>
        <div v-else>
            <v-toolbar class="blue" flat dense dark>
                <v-layout row align-center>
                    <v-toolbar-title>
                        User Story - {{currentWI.changed.number}}
                    </v-toolbar-title>
                    <v-spacer></v-spacer>
                    <v-flex md4>
                        <v-tabs
                            color="blue"
                            v-model="model"
                            centered
                            slider-color="black">
                            <v-tab ripple>
                                Комментарии
                            </v-tab>
                            <v-tab ripple>
                                Вложения
                            </v-tab>
                            <v-tab ripple>
                                История
                            </v-tab>
                        </v-tabs>
                    </v-flex>
                </v-layout>
            </v-toolbar>
            <v-layout row justify-end wrap fill-height>
                <v-flex md8 xs12>
                    <v-container class="pt-0 divide">
                        <v-layout column>
                            <v-text-field v-model="currentWI.changed.name">
                            </v-text-field>
                            <ckeditor :editor="editor" v-model="currentWI.changed.description.description" :config="editorConfig"></ckeditor>
                        </v-layout>  
                    </v-container>       
                </v-flex>
                <v-flex md4 xs12>
                    <v-tabs-items v-model="model">
                        <v-tab-item>
                            <v-card flat>
                            <v-card-text>Комментарии</v-card-text>
                            </v-card>
                        </v-tab-item>
                        <v-tab-item>
                            <files></files>
                        </v-tab-item>
                        <v-tab-item>
                            <v-card flat>
                            <v-card-text>История</v-card-text>
                            </v-card>
                        </v-tab-item>
                    </v-tabs-items>
                </v-flex>
            </v-layout>
        </div>
    </div>
</template>

<script>
import {mapGetters, mapMutations} from 'vuex'
import onResize from '../../../mixins/on-resize'
import files from '../files'
import ClassicEditor from '@ckeditor/ckeditor5-build-classic'
import adapters from '../../../image-upload-adapter'

export default {
    mixins:[onResize],
    components:{
        'files':files
    },
    mounted(){
        this.enter();
    },
    data:()=>({
        loading:true,
        model:null,
        editor: ClassicEditor,
        editorConfig: {
            extraPlugins: [ adapters.updateWIAdapter ]
        }
    }),
    computed:{
        number(){
            return this.$route.params.number;
        },
        ...mapGetters({currentWI:'project/currentWI'})
    },
    methods:{
        enter(){
            if(this.currentWI){
                this.loading=false;
            }
            else{
                this.loading = true;
                this.$http.get(`/api/workitems/project/${this.$route.params.projId}/item/STORY/${this.number}`)
                .then(
                    r=>{
                        r.data.changed = Object.assign({}, r.data);
                        this.addWI(r.data);
                        this.loading = false;
                    },
                    r=>console.log(r.response)
                );
            }
        },
        ...mapMutations({addWI:'project/addWI'})
    },
    watch:{
        number(from, to){
            if(!to)
                return;
            this.enter();
        }
    }
}
</script>

<style scoped>
.divide{
    border-right: 1px solid gray;
}
</style>


