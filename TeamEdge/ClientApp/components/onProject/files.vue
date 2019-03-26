<template>
    <div>
        <v-container v-show="loading">
            <v-layout column justify-center align-center fill-height>
                <v-progress-circular indeterminate color="primary"></v-progress-circular>
            </v-layout>
        </v-container>
        <div v-show="!loading">
            <v-layout justify-end >
                <v-btn small icon @click="selectFiles">
                    <v-icon>
                        attach_file
                    </v-icon>
                </v-btn>
                <v-btn small icon @click="fetchFiles">
                    <v-icon>
                        refresh
                    </v-icon>
                </v-btn>
            </v-layout>
            <v-layout row wrap justify-center>
                <v-card v-for="(f,i) in currentWI.description.files" :key="i" class="mx-1 my-1">
                    <v-card-text class="mx-1 my-1">
                        <img height="128px" v-if="f.isPicture" :src="f.path"/>
                        <v-icon v-else>
                            insert_drive_file
                        </v-icon>
                    </v-card-text>
                </v-card>
            </v-layout>
        </div>
    </div>
</template>

<script>
import { mapMutations } from 'vuex';
import currentItem from '../../mixins/work-item';
export default {
    mixins:[currentItem],
    mounted(){
        this.fetchFiles();
    },
    data:()=>({
        loading:false
    }),
    methods:{
        fetchFiles(){
            this.loading = true;
            this.$http.get(`/api/project/${this.$route.params.projId}/item/${this.currentWI.descriptionId}/files`)
            .then(
                r=>{
                    this.currentWI.description.files = r.data; 
                    this.currentWI.changed.description.files = null;
                    this.loading = false;
                },
                r=>console.log(r.response)
            );
        },
        selectFiles(){

        },
        ...mapMutations({open:'fileSelector/open'}),
    },
    watch:{
        itemId(from, to){
            this.fetchFiles();
        }
    }
}
</script>

<style>

</style>
