<template>
    <div class="block">
        <a href="#" @click="onFocus">
            <v-avatar size="90">
                <v-icon v-if="!image" size="90">account_circle</v-icon>
                <v-img v-else :src="image"/>
            </v-avatar>
        </a>
        <slot>
        </slot>
        <input type="file" accept="image/x-png,image/gif,image/jpeg,image/png" v-show="false"
            :multiple="false" ref="photoInput" @change="onPhotoChange">                    
    </div>

</template>

<script>
export default {
    props:['image'],
    methods:{
        onFocus(event){
            this.$refs.photoInput.click();
        },
        onPhotoChange($event){
            const files = $event.target.files || $event.dataTransfer.files;
            if (!files.length) 
                return;
            var fd  = new FormData();
            fd.append('file', files[0]);
            this.$http.post('/api/file/image', fd, { headers: {'Content-Type': 'multipart/form-data' }}).then(
                r=>{this.$emit('fotoLoaded', r.data)},
                r=>{console.log(r.responce);}
            );
        }
    },
    watch:{
        image(){
            this.$refs.photoInput.files = null;
        }
    }
}
</script>

<style scoped>
.block{
    display: flex;
}
</style>
